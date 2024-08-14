using Microsoft.AspNetCore.Identity;

namespace PhoneDirectory.Models
{
    public class AppUser:IdentityUser
    {
        public List<Person> Persons{ get; set; }=new List<Person>();

    }
}
