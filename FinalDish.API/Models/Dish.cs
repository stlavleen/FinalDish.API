using System.ComponentModel.DataAnnotations;

namespace FinalDish.API.Models
{
    public class Dish
    {
        [Key]
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public DishType Type { get; set; }
    }
}
