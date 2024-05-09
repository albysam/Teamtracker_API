using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ReactUser.Models.Dto
{
    public class MessageDTO
    {
        public string User { get; set; }
        public string Room { get; set; }
        public string Message { get; set; }
    }
}
