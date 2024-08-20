using Microsoft.IdentityModel.Tokens;
using PhoneDirectory.Dtos.PersonDtos;
using PhoneDirectory.Models;
using System.Collections.Generic;
using System.IO;

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

                PhoneNumber = createPersonDto.PhoneNumber.Select(phone => new PhoneNumber { PhoneNumberDetail = phone }).ToList()


            };
        }

        //public static Person ToPersonFromPersonDto(this PersonDto personDto)
        //{
        //    return new Person
        //    {
        //        FullName = personDto.FullName,
        //        Birthday = personDto.Birthday,
        //        IsFavourite = personDto.IsFavourite,
        //        Address = !string.IsNullOrEmpty(personDto.AddressDetail) ? new Address { AddressDetail = personDto.AddressDetail } : null,
        //        Email = !string.IsNullOrEmpty(personDto.EmailDetail) ? new Email { EmailDetail = personDto.EmailDetail } : null,
        //        PhoneNumber = personDto.PhoneNumber.Select(phone => new PhoneNumber { PhoneNumberDetail = phone }).ToList(),

        //    };

        //}
        public static PersonDto ToPersonDtoFromPerson(this Person person)
        {
           
            var PersonDto = new PersonDto
            {
                Id = person.Id,
                FullName = person.FullName,
                IsFavourite=person.IsFavourite,
                Birthday = person.Birthday,
                PhotoUrl = person.Photo==null ? null : person.Photo.PhotoDetail,
                AddressDetail = person.Address == null ? null : person.Address.AddressDetail,
                EmailDetail = person.Email == null ? null : person.Email.EmailDetail,
                PhoneNumber = person.PhoneNumber.Select(x => x.PhoneNumberDetail).ToList()

            };

           
            return PersonDto;
        }
        //public static Person ToPersonFromUpdatePersonDto(this UpdatePersonDto updatePersonDto)
        //{
        //    return new Person
        //    {
        //        FullName = updatePersonDto.FullName,
        //        Birthday = updatePersonDto.Birthday,
        //        IsFavourite = updatePersonDto.IsFavourite,
        //        Address = !string.IsNullOrEmpty(updatePersonDto.AddressDetail ) ? new Address { AddressDetail = updatePersonDto.AddressDetail } : null,
        //        Email = !string.IsNullOrEmpty(updatePersonDto.EmailDetail) ? new Email { EmailDetail = updatePersonDto.EmailDetail } : null,
        //        PhoneNumber = updatePersonDto.PhoneNumber.Select(phone => new PhoneNumber { PhoneNumberDetail = phone }).ToList(),
        //        Photo = updatePersonDto.PhotoDetail!=null ? new Photo { PhotoDetail = updatePersonDto.PhotoDetail } : null,


        //    };
        //}
    }
}
