using FinalDish.API.Models;
using FinalDish.API.POD;

namespace FinalDish.API.Extensions
{
    public static class QueryExtension
    {
        public static IQueryable<DishPOD> ToPOD(this IQueryable<Dish> from) 
        {
            return from.Select(x => x.ToPOD());
        }

        public static IQueryable<Dishes_IngredientsPOD> ToPOD(this IQueryable<Dishes_Ingredients> from) 
        {
            return from.Select(x => x.ToPOD());
        }
    }
}
