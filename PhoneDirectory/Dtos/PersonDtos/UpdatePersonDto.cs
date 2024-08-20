using PhoneDirectory.Models;

namespace PhoneDirectory.Dtos.PersonDtos
{
    public class UpdatePersonDto
    {
        public string FullName { get; set; } = string.Empty;
        public bool? IsFavourite { get; set; }
        public List<string> PhoneNumber { get; set; } = new List<string>();
        public IFormFile? PhotoUrl { get; set; }
        public string? Birthday { get; set; }
        public String? AddressDetail { get; set; }
        public String? EmailDetail { get; set; }
    }
}
