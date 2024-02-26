using Microsoft.EntityFrameworkCore;

namespace SignalR_Chat.Models
{
    public class ChatContext: DbContext
    {
        public DbSet<Users> Users { get; set; }
        
        public ChatContext(DbContextOptions<ChatContext> options)
            : base(options)
        {
            Database.EnsureCreated(); 
        }

        public async void SaveMessage(string username,string message, string conId)
        {
            Users users = new Users
            {
                Message = message,
                Name = username,
                Created = DateTime.UtcNow,
                ConnectionId = conId
            };
            if(users != null ) 
             await Users.AddAsync(users);
            SaveChanges();
        }
      
        public async Task<List<Users>> GetUsers()
        {
            return await Users.ToListAsync(); 
        }
    }
}
