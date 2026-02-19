
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
    [Route("[controller]/[action]")]
    [ApiController]
    public class DishesController : IntermediateController
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<AppUser> userManager;

        public DishesController
            (ApplicationDbContext context, 
            UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = CacheProfilesNames.NoStore)]
        public async Task<IEnumerable<Dish>> GetBaseDishes([FromQuery] RangeRequestDTO data)
        {
            return await context.Dishes
                .Where(x => x.UserId == null)
                .Skip(data.RangeId * data.RangeSize)
                .Take(data.RangeSize)
                .ToArrayAsync();
        }

        [Authorize(Roles = RolesNames.Moderator)]
        [HttpGet]
        [ResponseCache(CacheProfileName = CacheProfilesNames.NoStore)]
        public async Task<IEnumerable<Dish>> GetAll([FromQuery] RangeRequestDTO data) 
        {
            return await context.Dishes
                .Skip(data.RangeId * data.RangeSize)
                .Take(data.RangeSize)
                .ToArrayAsync();
        }
        
        [Authorize]
        [HttpGet]
        [ResponseCache(CacheProfileName = CacheProfilesNames.NoStore)]
        public async Task<IActionResult> GetUserDishes(string? userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (!IsAuthorizedRequest(user))
                return Forbid();

            var dishes = await context.Dishes
                .Where(x => x.UserId == userId)
                .ToPOD()
                .ToArrayAsync();

            return JsonResponse(dishes);
        }

        [Authorize]
        [HttpPost]
        [ResponseCache(CacheProfileName = CacheProfilesNames.NoStore)]
        public async Task<IActionResult> Post(DishDTO data) 
        {
            try
            {
                if (context.DishTypes.Find(data.DishTypeId) is null)
                    return Error500($"Dish type with id = {data.DishTypeId} does not exist");

                var user = await userManager.FindByIdAsync(data.UserId);

                if (data.UserId is not null && user is null)
                    return Error500($"User with id = {data.UserId} does not exist");

                if (!IsAuthorizedRequest(user))
                    return Forbid();

                var dish = new Dish 
                {
                    Name = data.Name,
                    DishTypeId = data.DishTypeId,
                    UserId = data.UserId
                };
                var entry = await context.Dishes.AddAsync(dish);
                await context.SaveChangesAsync();

                return Created(string.Empty, $"Dish with id = {entry.Entity.Id} has been created.");
            }
            catch (Exception ex) 
            {
                return Error500(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [ResponseCache(CacheProfileName = CacheProfilesNames.NoStore)]
        public async Task<IActionResult> Put(int id, DishDTO data)
        {
            try
            {
                if (context.DishTypes.Find(data.DishTypeId) is null)
                    return Error500($"Dish type with id = {data.DishTypeId} does not exist");

                var user = await userManager.FindByIdAsync(data.UserId);

                if (data.UserId is not null && user is null)
                    return Error500($"User with id = {data.UserId} does not exist");

                if (!IsAuthorizedRequest(user))
                    return Forbid();

                var entity = await context.Dishes.FirstOrDefaultAsync(x => x.Id == id);

                if (entity is not null)
                {
                    entity.Name = data.Name;
                    entity.DishTypeId = data.DishTypeId;

                    await context.SaveChangesAsync();

                    return Ok($"Dish with id = {id} has been updated.");
                }
                else 
                {
                    return BadRequest($"Dish with id = {id} does not exist.");
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

        [Authorize]
        [HttpDelete]
        [ResponseCache(CacheProfileName = CacheProfilesNames.NoStore)]
        public async Task<IActionResult> Delete(int id) 
        {
            try
            {
                var entity = await context.Dishes.FirstOrDefaultAsync(x => x.Id == id);

                if (entity is not null)
                {
                    var user = await userManager.FindByIdAsync(entity.UserId);

                    if (!IsAuthorizedRequest(user))
                        return Forbid();

                    var entry = context.Dishes.Remove(entity);
                    await context.SaveChangesAsync();

                    return Ok($"Dish with id = {id} has been removed.");
                }
                else
                {
                    return BadRequest($"Dish with id = {id} does not exist.");
                }
            }
            catch (Exception ex) 
            {
                return Error500(ex.Message);
            }
        }
    }
}
