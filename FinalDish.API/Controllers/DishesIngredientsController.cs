
using FinalDish.API.Constants;
using FinalDish.API.DTO;
using FinalDish.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalDish.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DishesIngredientsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public DishesIngredientsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = CacheProfilesNames.NoStore)]
        public async Task<Dishes_Ingredients[]> Get(int dishId) 
        {
            return await context.Dishes_Ingredients
                .Where(x => x.DishId == dishId)
                .ToArrayAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post(DishIngredientDTO data) 
        {
            try
            {
                var entity = new Dishes_Ingredients 
                {
                    DishId = data.DishId,
                    IngredientId = data.IngredientId
                };
                await context.Dishes_Ingredients.AddAsync(entity);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status201Created, 
                    $"Ingredient with id = {data.IngredientId} has been added to dish with id = {data.DishId}.");
            }
            catch (Exception ex) 
            {
                var details = new ProblemDetails();
                details.Detail = ex.Message;
                details.Status = StatusCodes.Status500InternalServerError;

                return StatusCode(StatusCodes.Status500InternalServerError, details);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(DishIngredientDTO data)
        {
            try
            {
                var entity = await context.Dishes_Ingredients
                    .FirstOrDefaultAsync(x => x.DishId == data.DishId && 
                    x.IngredientId == data.IngredientId);

                if (entity is not null)
                {
                    var entry = context.Dishes_Ingredients.Remove(entity);
                    await context.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status200OK, 
                        $"Ingredient with id = {data.IngredientId} has been removed from dish with id = {data.DishId}.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, 
                        $"Dish with id = {data.DishId} does not contain ingredient with id = {data.IngredientId}.");
                }
            }
            catch (Exception ex)
            {
                var details = new ProblemDetails();
                details.Detail = ex.Message;
                details.Status = StatusCodes.Status500InternalServerError;

                return StatusCode(StatusCodes.Status500InternalServerError, details);
            }
        }
    }
}
