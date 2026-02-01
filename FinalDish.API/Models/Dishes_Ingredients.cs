
using System.ComponentModel.DataAnnotations;

namespace FinalDish.API.Models
{
    public class Dishes_Ingredients
    {
        [Key]
        [Required]
        public int DishId { get; set; }

        [Key]
        [Required]
        public int IngredientId { get; set; }

        public Dish? Dish { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}
