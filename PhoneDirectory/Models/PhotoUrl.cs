namespace PhoneDirectory.Models
{
    public class PhotoUrl:BaseEntity
    {
        public string? PhotoUrlDetail { get; set; } = String.Empty;
        public int PersonId { get; set; }
        public Person? Person { get; set; }
    }
}
