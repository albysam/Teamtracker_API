namespace Talento_API.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
