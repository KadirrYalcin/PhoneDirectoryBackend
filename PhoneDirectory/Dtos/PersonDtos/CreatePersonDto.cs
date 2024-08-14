using PhoneDirectory.Models;

namespace PhoneDirectory.Dtos.PersonDtos
{
    public class CreatePersonDto
    {
        public string FullName { get; set; } = string.Empty;
        public List<string> PhoneNumber { get; set; } = new List<string>();

        // Opsiyonel alanlar
        public string? PhotoUrl { get; set; }
        public string? Birthday { get; set; }
        public string? AddressDetail { get; set; }
        public string? EmailDetail { get; set; }

    }
}
