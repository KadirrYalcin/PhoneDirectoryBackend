namespace PhoneDirectory.Models
{
    public class Email:BaseEntity
    {
        public string? EmailDetail { get; set; }=String.Empty;
        public int PersonId { get; set; }
        public Person? Person { get; set; }
    }
}
