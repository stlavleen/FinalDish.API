
using FinalDish.API.Constants;
using FinalDish.API.DTO;
using FinalDish.API.Extensions;
using FinalDish.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalDish.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DishesIngredientsController : IntermediateController
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;

        public DishesIngredientsController
            (ApplicationDbContext context, 
            UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        [ResponseCache(CacheProfileName = CacheProfilesNames.NoStore)]
        public async Task<IActionResult> Get(int dishId) 
        {
            var dish = await context.Dishes.FindAsync(dishId);

            if (dish is not null) 
            {
                var user = await userManager.FindByIdAsync(dish.UserId);

                if (!IsAuthorizedRequest(user))
                    return Forbid();
            }

            var dishesIngredients = await context.Dishes_Ingredients
                .Where(x => x.DishId == dishId)
                .ToPOD()
                .ToArrayAsync();

            return JsonResponse(dishesIngredients);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(DishIngredientDTO data) 
        {
            try
            {
                var dish = await context.Dishes.FindAsync(data.DishId);

                if (dish is not null)
                {
                    var user = await userManager.FindByIdAsync(dish.UserId);

                    if (!IsAuthorizedRequest(user))
                        return Forbid();
                }

                var entity = new Dishes_Ingredients 
                {
                    DishId = data.DishId,
                    IngredientId = data.IngredientId
                };
                await context.Dishes_Ingredients.AddAsync(entity);
                await context.SaveChangesAsync();

                return Created(string.Empty, 
                    $"Ingredient with id = {data.IngredientId} has been added to dish with id = {data.DishId}.");
            }
            catch (Exception ex) 
            {
                return Error500(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(DishIngredientDTO data)
        {
            try
            {
                var dish = await context.Dishes.FindAsync(data.DishId);

                if (dish is not null)
                {
                    var user = await userManager.FindByIdAsync(dish.UserId);

                    if (!IsAuthorizedRequest(user))
                        return Forbid();
                }

                var entity = await context.Dishes_Ingredients
                    .FirstOrDefaultAsync(x => x.DishId == data.DishId && 
                    x.IngredientId == data.IngredientId);

                if (entity is not null)
                {
                    var entry = context.Dishes_Ingredients.Remove(entity);
                    await context.SaveChangesAsync();

                    return Ok($"Ingredient with id = {data.IngredientId} has been removed from dish with id = {data.DishId}.");
                }
                else
                    return BadRequest($"Dish with id = {data.DishId} does not contain ingredient with id = {data.IngredientId}.");
            }
            catch (Exception ex)
            {
                return Error500(ex.Message);
            }
        }
    }
}
