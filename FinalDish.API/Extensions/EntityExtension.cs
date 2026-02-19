
using FinalDish.API.Models;
using FinalDish.API.POD;

namespace FinalDish.API.Extensions
{
    public static class EntityExtension
    {
        public static DishPOD ToPOD(this Dish from) 
        {
            return new DishPOD 
            {
                Id = from.Id,
                Name = from.Name,
                DishTypeId = from.DishTypeId,
                UserId = from.UserId
            };
        }

        public static Dishes_IngredientsPOD ToPOD(this Dishes_Ingredients from) 
        {
            return new Dishes_IngredientsPOD 
            {
                DishId = from.DishId,
                IngredientId = from.IngredientId
            };
        }
    }
}
