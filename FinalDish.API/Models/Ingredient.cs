

using System.ComponentModel.DataAnnotations;

namespace FinalDish.API.Models
{
    public class Ingredient
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public ICollection<Dishes_Ingredients>? Dishes_Ingredients { get; set; }
    }
}
