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

        [HttpGet("GetDishes")]
        public async Task<IEnumerable<Dish>> Get() 
        {
            return await context.Dishes.ToArrayAsync();
        }
    }
}
