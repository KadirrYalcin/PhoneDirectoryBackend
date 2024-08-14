using PhoneDirectory.Dtos.PersonDtos;
using PhoneDirectory.Models;

namespace PhoneDirectory.Services
{
    public interface PersonService
    {
        Task<List<PersonDto>?>getAll(AppUser user);
        Task<PersonDto>? getPerson();
        Task<Person> createPerson(Person person);
        Task<Person> updatePerson(UpdatePersonDto updatePersonDto,int personId);
        Task DeletePerson(Person DeletePerson);
        Task<Person> GetPersonById(int id);
    }
}
