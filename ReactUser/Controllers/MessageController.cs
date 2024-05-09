using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ReactUser.Data;
using Talento_API.Chats;
using Talento_API.Hubs;

namespace Talento_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageController(ApplicationDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet("{senderId}/{receiverId}")]
        public async Task<IActionResult> GetMessages(string senderId, string receiverId)
        {
            try
            {
                var messages = await _context.ChatMessages
                    .Where(m => (m.Sender == senderId && m.Receiver == receiverId) ||
                                (m.Sender == receiverId && m.Receiver == senderId))
                    .ToListAsync();

                if (messages.Any())
                {
                    await _hubContext.Clients.User(receiverId).SendAsync("ReceiveNotification", "New Message", "You have received a new message.");
                }
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}











