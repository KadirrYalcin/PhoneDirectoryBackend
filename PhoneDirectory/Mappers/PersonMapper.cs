using Microsoft.IdentityModel.Tokens;
using PhoneDirectory.Dtos.PersonDtos;
using PhoneDirectory.Models;
using System.Collections.Generic;

namespace PhoneDirectory.Mappers
{
    public static class PersonMapper
    {
        public static Person ToPersonFromCreatePersonDto(this CreatePersonDto createPersonDto)
        {
            return new Person
            {
                FullName = createPersonDto.FullName,
                Birthday = createPersonDto.Birthday,
                IsFavourite = false,
                Address = !string.IsNullOrEmpty(createPersonDto.AddressDetail) ? new Address { AddressDetail = createPersonDto.AddressDetail } : null,
                Email = !string.IsNullOrEmpty(createPersonDto.EmailDetail) ? new Email { EmailDetail = createPersonDto.EmailDetail } : null,
                PhotoUrl = !string.IsNullOrEmpty(createPersonDto.PhotoUrl) ? new PhotoUrl { PhotoUrlDetail = createPersonDto.PhotoUrl } : null,
                PhoneNumber = createPersonDto.PhoneNumber.Select(phone => new PhoneNumber { PhoneNumberDetail = phone }).ToList()


            };
        }

        public static Person ToPersonFromPersonDto(this PersonDto personDto)
        {
            return  new Person
            {
                FullName = personDto.FullName,
                Birthday = personDto.Birthday,
                IsFavourite = personDto.IsFavourite,
                AppUserId = personDto.AppUserId,
                Address = !string.IsNullOrEmpty(personDto.AddressDetail) ? new Address { AddressDetail = personDto.AddressDetail } : null,
                Email = !string.IsNullOrEmpty(personDto.EmailDetail) ? new Email { EmailDetail = personDto.EmailDetail } : null,
                PhoneNumber = personDto.PhoneNumber.Select(phone => new PhoneNumber { PhoneNumberDetail = phone }).ToList(),
                PhotoUrl = !string.IsNullOrEmpty(personDto.PhotoUrl) ? new PhotoUrl { PhotoUrlDetail = personDto.PhotoUrl } : null,
            };
           
        }
        public static PersonDto ToPersonDtoFromPerson(this Person person)
        {
            var PersonDto = new PersonDto
            {
                Id = person.Id,
                FullName = person.FullName,
                AppUserId = person.AppUserId,
                IsFavourite=person.IsFavourite,
                Birthday = person.Birthday,
                PhotoUrl = person.PhotoUrl==null ? null : person.PhotoUrl.PhotoUrlDetail,
                AddressDetail = person.Address == null ? null : person.Address.AddressDetail,
                EmailDetail = person.Email == null ? null : person.Email.EmailDetail,
                PhoneNumber = person.PhoneNumber.Select(x => x.PhoneNumberDetail).ToList()

            };

           
            return PersonDto;
        }
        public static Person ToPersonFromUpdatePersonDto(this UpdatePersonDto updatePersonDto)
        {
            return new Person
            {
                FullName = updatePersonDto.FullName,
                Birthday = updatePersonDto.Birthday,
                IsFavourite = updatePersonDto.IsFavourite,
                Address = !string.IsNullOrEmpty(updatePersonDto.AddressDetail ) ? new Address { AddressDetail = updatePersonDto.AddressDetail } : null,
                Email = !string.IsNullOrEmpty(updatePersonDto.EmailDetail) ? new Email { EmailDetail = updatePersonDto.EmailDetail } : null,
                PhoneNumber = updatePersonDto.PhoneNumber.Select(phone => new PhoneNumber { PhoneNumberDetail = phone }).ToList(),
                PhotoUrl = !string.IsNullOrEmpty(updatePersonDto.PhotoUrl) ? new PhotoUrl { PhotoUrlDetail = updatePersonDto.PhotoUrl } : null,


            };
        }
    }
}
