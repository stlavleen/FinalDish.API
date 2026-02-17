
using Microsoft.AspNetCore.Identity;

namespace FinalDish.API.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<Dish>? Dishes { get; set; }
    }
}
