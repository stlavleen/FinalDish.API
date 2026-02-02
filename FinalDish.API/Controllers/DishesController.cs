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

        [HttpGet("GetRange")]
        public async Task<IEnumerable<Dish>> Get([FromQuery] RangeRequestDTO request) 
        {
            return await context.Dishes
                .Skip(request.RangeId * request.RangeSize)
                .Take(request.RangeSize)
                .ToArrayAsync();
        }
    }
}
