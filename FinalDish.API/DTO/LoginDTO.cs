
using System.ComponentModel.DataAnnotations;

namespace FinalDish.API.DTO
{
    public class LoginDTO
    {
        [MinLength(6)]
        [MaxLength(20)]
        public string? Name { get; set; }

        [MinLength(6)]
        public string? Password { get; set; }
    }
}
