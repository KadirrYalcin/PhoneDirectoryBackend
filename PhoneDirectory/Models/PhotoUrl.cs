namespace PhoneDirectory.Models
{
    public class Photo:BaseEntity
    {
        public String? PhotoDetail { get; set; }
        public int PersonId { get; set; }
        public Person? Person { get; set; }
    }
}
