using Microsoft.EntityFrameworkCore;

namespace FinalDish.API.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Dish> Dishes => Set<Dish>();
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    }
}
