

namespace FinalDish.API.POD
{
    /// <summary>
    /// Plain old data object without navigation fields
    /// </summary>
    public class DishPOD
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int DishTypeId { get; set; }

        public string? UserId { get; set; }
    }
}
