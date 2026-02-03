using FinalDish.API.DTO;
using FinalDish.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalDish.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public DishesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Dish>> Get([FromQuery] RangeRequestDTO data) 
        {
            return await context.Dishes
                .Skip(data.RangeId * data.RangeSize)
                .Take(data.RangeSize)
                .ToArrayAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post(DishDTO data) 
        {
            try
            {
                var dish = new Dish 
                {
                    Name = data.Name,
                    DishTypeId = data.DishTypeId
                };
                var entry = await context.Dishes.AddAsync(dish);
                return StatusCode(StatusCodes.Status201Created,
                    new JsonResult(new 
                    {
                        DishId = entry.Entity.Id,
                        Message = "Dish has been created."
                    }));
            }
            catch (Exception ex) 
            {
                var details = new ProblemDetails();
                details.Detail = ex.Message;
                details.Status = StatusCodes.Status500InternalServerError;

                return StatusCode(StatusCodes.Status500InternalServerError, details);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, DishDTO data)
        {
            try
            {
                var entity = await context.Dishes.FirstOrDefaultAsync(x => x.Id == id);

                if (entity is not null)
                {
                    entity.Name = data.Name;
                    entity.DishTypeId = data.DishTypeId;

                    await context.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status200OK, $"Dish with id = {id} has been updated.");
                }
                else 
                {
                    return StatusCode(StatusCodes.Status400BadRequest, $"Dish with id = {id} does not exist.");
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

        [HttpDelete]
        public async Task<IActionResult> Delete(int id) 
        {
            try
            {
                var entity = await context.Dishes.FirstOrDefaultAsync(x => x.Id == id);

                if (entity is not null)
                {
                    var entry = context.Dishes.Remove(entity);

                    return StatusCode(StatusCodes.Status200OK, $"Dish with id = {id} has been removed.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, $"Dish with id = {id} does not exist.");
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
