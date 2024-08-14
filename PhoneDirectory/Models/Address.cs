namespace PhoneDirectory.Models
{
    public class Address:BaseEntity
    {
        public string? AddressDetail { get; set; } = String.Empty;
        public int PersonId { get; set; }
        public Person? Person{ get; set; }
    }
}
