namespace SignalR_Chat.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string? ConnectionId { get; set; }
        public string? Name { get; set; }

        public string Message { get; set; }

        public DateTime? Created { get; set; }
    }
}
