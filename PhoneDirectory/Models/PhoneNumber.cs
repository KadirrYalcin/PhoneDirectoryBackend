namespace PhoneDirectory.Models
{
    public class PhoneNumber:BaseEntity
    {
        public string PhoneNumberDetail { get; set; } = String.Empty;
        public int PersonId { get; set; }
        public Person? Person { get; set; }
    }
}
