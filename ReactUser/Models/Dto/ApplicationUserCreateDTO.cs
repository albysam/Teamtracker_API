using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ReactUser.Models.Dto
{
    public class ApplicationUserCreateDTO
    {
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        public IFormFile File { get; set; }
    }
}
