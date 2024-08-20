using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using PhoneDirectory.Data;
using PhoneDirectory.Dtos.PersonDtos;
using PhoneDirectory.Mappers;
using PhoneDirectory.Models;
using PhoneDirectory.Services;
using System;
using System.IO;

namespace PhoneDirectory.Repostory
{
    public class PersonRepostory : PersonService
    {
        private readonly ApplicationDBContext _dbContext;
        public PersonRepostory(ApplicationDBContext applicationDBContext, IWebHostEnvironment environment)
        {
            _dbContext = applicationDBContext;

        }

        public async Task<Person> createPerson(Person person)
        {
            if (person == null)
            {
                return null; // Eğer kişi bulunamazsa, null döndür
            }
            else if (string.IsNullOrEmpty(person.FullName))
            {
                throw new ArgumentException("Full Name cannot be empty.", nameof(person.FullName));
            }
            if (person.PhoneNumber == null || !person.PhoneNumber.Any())
            {
                throw new ArgumentException("phone number must be valid.", nameof(person.FullName));
            }

            if (person.Email != null && person.Email.Id == 0)
            {
                await _dbContext.Emails.AddAsync(person.Email);
            }

            if (person.Address != null && person.Address.Id == 0)
            {
                await _dbContext.Addresses.AddAsync(person.Address);
            }
            if (person.Photo != null && person.Photo.Id == 0)
            {
                await _dbContext.Photos.AddAsync(person.Photo);
            }

            // Şimdi person nesnesini ekle
            await _dbContext.Persons.AddAsync(person);
            await _dbContext.SaveChangesAsync();
            return person;
        }

        public async Task DeletePerson(Person DeletePerson)
        {
            {
                // Öncelikle Person nesnesini ve ilişkili verileri bul
                var person = await _dbContext.Persons
                    .Include(p => p.Email)
                    .Include(p => p.Address)
                    .Include(p => p.Photo)
                    .Include(p => p.PhoneNumber)
                    .FirstOrDefaultAsync(p => p.Id == DeletePerson.Id);

                if (person == null)
                {
                    throw new ArgumentException("Person not found.");
                }

                // İlişkili PhoneNumber verilerini sil
                if (person.PhoneNumber != null)
                {
                    _dbContext.PhoneNumbers.RemoveRange(person.PhoneNumber);
                }

                // İlişkili Email verisini sil
                if (person.Email != null)
                {
                    _dbContext.Emails.Remove(person.Email);
                }

                // İlişkili Address verisini sil
                if (person.Address != null)
                {
                    _dbContext.Addresses.Remove(person.Address);
                }

                // İlişkili Photo verisini sil
                if (person.Photo != null)
                {
                    _dbContext.Photos.Remove(person.Photo);
                }

                // Son olarak Person nesnesini sil
                _dbContext.Persons.Remove(person);

                // Değişiklikleri veritabanına kaydet
                await _dbContext.SaveChangesAsync();

            }
        }

        public async Task<List<PersonDto>?> getAll(AppUser user)
        {
            List<PersonDto> personDtoList = new List<PersonDto>();

            var personList = await _dbContext.Persons
                .Where(p => p.AppUserId == user.Id)
                .Include(p => p.PhoneNumber)
                .Include(p => p.Address)
                .Include(p => p.Email)
                .Include(p => p.Photo)
                .ToListAsync();

            foreach (var item in personList)
            {

          
                PersonDto personDto = item.ToPersonDtoFromPerson();

                personDtoList.Add(personDto);
            }

            return personDtoList;
        }


        public async Task<Person> GetPersonById(int id)
        {
            var person = await _dbContext.Persons
                   .Include(p => p.PhoneNumber)
                   .Include(p => p.Address)
                   .Include(p => p.Email)
                   .Include(p => p.Photo)
                   .FirstOrDefaultAsync(p => p.Id == id);

            return person;
        }

        public async Task<Person> updatePerson(UpdatePersonDto updatePersonDto, int PersonId)
        {
            // Person nesnesini, ilgili ilişkilerle birlikte veritabanından al
            var person = await _dbContext.Persons
                .Include(p => p.Email)
                .Include(p => p.Address)
                .Include(p => p.PhoneNumber)
                .Include(p => p.Photo)
                .FirstOrDefaultAsync(p => p.Id == PersonId);
            if (person == null)
            {
                return null; // Eğer kişi bulunamazsa, null döndür
            }
            else if (string.IsNullOrEmpty(updatePersonDto.FullName))
            {
                throw new ArgumentException("Full Name cannot be empty.", nameof(updatePersonDto.FullName));
            }
            if (updatePersonDto.PhoneNumber == null || !updatePersonDto.PhoneNumber.Any())
            {
                throw new ArgumentException("phone number must be valid.", nameof(updatePersonDto.FullName));
            }


            // Email güncelleme
            if (person.Email != null)
            {
                person.Email.EmailDetail = updatePersonDto.EmailDetail;
                person.Email.UpdateDate = DateTime.Now;
            }

            else if (!string.IsNullOrEmpty(updatePersonDto.EmailDetail))
            {
                var newEmail = new Email { EmailDetail = updatePersonDto.EmailDetail };
                person.Email = newEmail;
                await _dbContext.Emails.AddAsync(newEmail);
                await _dbContext.SaveChangesAsync();
            }

            // Address güncelleme
            if (person.Address != null)
            {
                person.Address.AddressDetail = updatePersonDto.AddressDetail;
                person.Address.UpdateDate = DateTime.Now;
            }
            else if (!string.IsNullOrEmpty(updatePersonDto.AddressDetail))
            {
                var newAddress = new Address { AddressDetail = updatePersonDto.AddressDetail };
                person.Address = newAddress;
                await _dbContext.Addresses.AddAsync(newAddress);
                await _dbContext.SaveChangesAsync();
            }

            // Fotoğraf yolunu güncelleme işlemi
            if (person.Photo != null)
            {
                if (updatePersonDto.PhotoUrl != null)
                {
                    var extension = Path.GetExtension(updatePersonDto.PhotoUrl.FileName);
                    var newname = person.Id + extension;
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var location = Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder, newname);

                    var stream = new FileStream(location, FileMode.Create);
                    updatePersonDto.PhotoUrl.CopyTo(stream);
                    Photo photo = new Photo { PhotoDetail = newname };
                    person.Photo = photo;
                }
            }
  

                // PhoneNumber güncelleme (bu durumda, önceki telefon numaralarını temizle ve yenilerini ekle)
                if (updatePersonDto.PhoneNumber != null)
                {
                    // Önceki telefon numaralarını sil
                    var existingPhoneNumbers = _dbContext.PhoneNumbers.Where(pn => pn.PersonId == person.Id);
                    _dbContext.PhoneNumbers.RemoveRange(existingPhoneNumbers);

                    // Yeni telefon numaralarını ekle
                    var newPhoneNumbers = updatePersonDto.PhoneNumber.Select(phone => new PhoneNumber
                    {
                        PhoneNumberDetail = phone,
                        PersonId = person.Id
                    }).ToList();
                    await _dbContext.PhoneNumbers.AddRangeAsync(newPhoneNumbers);
                }

                // Diğer alanları güncelle
                person.FullName = updatePersonDto.FullName;
                person.Birthday = updatePersonDto.Birthday;
                person.IsFavourite = updatePersonDto.IsFavourite; // Eğer varsa
                person.UpdateDate = DateTime.Now;

                // Person nesnesini güncelle ve değişiklikleri kaydet
                _dbContext.Persons.Update(person);
                await _dbContext.SaveChangesAsync();

                return person;
            }

        }

    
}
