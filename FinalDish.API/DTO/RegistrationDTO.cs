using System.ComponentModel.DataAnnotations;

namespace FinalDish.API.DTO
{
    public class RegistrationDTO
    {
        [MinLength(6)]
        [MaxLength(20)]
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [MinLength(6)]
        public string? Password { get; set; }
    }
}
