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
        public int DishTypeId { get; set; }

        public DishType? Type { get; set; }

        public ICollection<Dishes_Ingredients>? Dishes_Ingredients { get; set; }
    }
}
