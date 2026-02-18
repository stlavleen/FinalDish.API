using FinalDish.API.Models;
using FinalDish.API.POD;

namespace FinalDish.API.Extensions
{
    public static class DishQueryExtension
    {
        public static IQueryable<DishPOD> ToPOD(this IQueryable<Dish> dishes) 
        {
            return dishes.Select(x => new DishPOD
            {
                Id = x.Id,
                Name = x.Name,
                DishTypeId = x.DishTypeId,
                UserId = x.UserId
            });
        }
    }
}
