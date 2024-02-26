using Microsoft.EntityFrameworkCore;
using SignalR_Chat;
using SignalR_Chat.Models;   // пространство имен класса ChatHub

var builder = WebApplication.CreateBuilder(args);
// ƒл€ использовани€ функциональности библиотеки SignalR,
// в приложении необходимо зарегистрировать соответствующие сервисы
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ChatContext>(options => options.UseSqlServer(connection));
builder.Services.AddSignalR();  

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<ChatHub>("/chat");   // ChatHub будет обрабатывать запросы по пути /chat

app.Run();
