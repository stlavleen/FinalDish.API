
using System.ComponentModel.DataAnnotations;

namespace FinalDish.API.Models
{
    public class DishType
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public ICollection<Dish>? Dishes { get; set; }
    }
}
