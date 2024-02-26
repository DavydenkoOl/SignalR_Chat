using Microsoft.AspNetCore.SignalR;
using SignalR_Chat.Models;

namespace SignalR_Chat
{
    /*
    Ключевой сущностью в SignalR, через которую клиенты обмениваются сообщеними 
    с сервером и между собой, является хаб (hub). 
    Хаб представляет некоторый класс, который унаследован от абстрактного класса 
    Microsoft.AspNetCore.SignalR.Hub.
    */
    public class ChatHub : Hub
    {
       
        public ChatContext chatContext { get; set; }
        // Отправка сообщений
       public ChatHub(ChatContext chatContext)
        {
            this.chatContext = chatContext;
        }
        static List<Users>? _User = new List<Users>();
        

        public async Task Send(string username, string message, string usId)
        {
            chatContext.SaveMessage(username, message, usId);
            
            await Clients.All.SendAsync("AddMessage", username, message);
           
        }

        // Подключение нового пользователя
        public async Task Connect(string userName)
        {
            _User = chatContext.GetUsers().Result;
            var id = Context.ConnectionId;

            if (!_User.Any(x => x.ConnectionId == id))
            {
                _User.Add(new Users { ConnectionId = id, Name = userName });
                chatContext.SaveChanges();

                await Clients.Caller.SendAsync("Connected", id, userName, _User);

              
                await Clients.AllExcept(id).SendAsync("NewUserConnected", id, userName);
            }
        }

        // OnDisconnectedAsync срабатывает при отключении клиента.
        // В качестве параметра передается сообщение об ошибке, которая описывает,
        // почему произошло отключение.
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var item = _User.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                _User.Remove(item);
                var id = Context.ConnectionId;
                // Вызов метода UserDisconnected на всех клиентах
                await Clients.All.SendAsync("UserDisconnected", id, item.Name);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
