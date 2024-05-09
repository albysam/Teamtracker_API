using Microsoft.AspNetCore.SignalR;
using ReactUser.Data;
using Talento_API.Models;

namespace Talento_API.Chats
{
    public class ChatHubb : Hub
    {
        private readonly ApplicationDbContext _context;
        public ChatHubb(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string sender, string receiver, string message)
        {
            var chatMessage = new ChatMessage
            {
                Sender = sender,
                Receiver = receiver,
                Message = message,
                Timestamp = DateTime.Now
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();



            await Clients.All.SendAsync("ReceiveMessage", sender, message, DateTime.Now.ToShortTimeString());

            // await Clients.User(receiver).SendAsync("ReceiveMessage", sender, message, DateTime.Now.ToShortTimeString());


            await Clients.User(receiver).SendAsync("ReceiveNotification", "New Message", "You have received a new message."); 
            await Clients.User(sender).SendAsync("ReceiveNotification", "Message Sent", "Your message was sent successfully.");
        }
    }


}