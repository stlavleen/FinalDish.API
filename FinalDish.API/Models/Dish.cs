namespace FinalDish.API.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Content { get; set; } = null!;
        public DishType Type { get; set; }
    }
}
