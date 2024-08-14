namespace PhoneDirectory.Models
{
    public class Person:BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string? Birthday { get; set; } = string.Empty;
        public bool? IsFavourite { get; set; }
        public string AppUserId { get; set; }
        public AppUser? Appuser { get; set; }


        public List<PhoneNumber> PhoneNumber { get; set; } = new List<PhoneNumber>();
        public Address? Address { get; set; }
        public PhotoUrl? PhotoUrl { get; set; }
        public Email? Email { get; set; }   
    }
}

