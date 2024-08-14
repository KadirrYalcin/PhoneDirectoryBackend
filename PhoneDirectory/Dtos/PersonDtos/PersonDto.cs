using PhoneDirectory.Models;

namespace PhoneDirectory.Dtos.PersonDtos
{
    public class PersonDto
    {
        public int? Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Birthday { get; set; } = string.Empty;
        public bool? IsFavourite { get; set; }
        public string AppUserId { get; set; }
        public List<String> PhoneNumber { get; set; } = new List<String>();
        public string? PhotoUrl { get; set; }
        public string? AddressDetail { get; set; }
        public string? EmailDetail { get; set; }
    }
}
