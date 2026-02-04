
using FinalDish.API.Constants;
using FinalDish.API.DTO;
using FinalDish.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalDish.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public IngredientsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("GetRange")]
        [ResponseCache(CacheProfileName = CacheProfilesNames.MaxAge300)]
        public async Task<Ingredient[]> Get([FromQuery] RangeRequestDTO data)
        {
            return await context.Ingredients
                .Skip(data.RangeId * data.RangeSize)
                .Take(data.RangeSize)
                .ToArrayAsync();
        }
    }
}
