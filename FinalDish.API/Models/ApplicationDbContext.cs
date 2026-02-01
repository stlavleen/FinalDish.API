using Microsoft.EntityFrameworkCore;

namespace FinalDish.API.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dishes_Ingredients>()
                .HasKey(x => new { x.DishId, x.IngredientId });

            modelBuilder.Entity<Dishes_Ingredients>()
                .HasOne(x => x.Dish)
                .WithMany(x => x.Dishes_Ingredients)
                .HasForeignKey(x => x.DishId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Dishes_Ingredients>()
                .HasOne(x => x.Ingredient)
                .WithMany(x => x.Dishes_Ingredients)
                .HasForeignKey(x => x.IngredientId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Dish> Dishes => Set<Dish>();
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<Dishes_Ingredients> Dishes_Ingredients => Set<Dishes_Ingredients>();
    }
}
