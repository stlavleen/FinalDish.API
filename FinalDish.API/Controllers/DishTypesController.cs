
using FinalDish.API.DTO;
using FinalDish.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalDish.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DishTypesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public DishTypesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("GetRange")]
        public async Task<DishType[]> Get([FromQuery] RangeRequestDTO data) 
        {
            return await context.DishTypes
                .Skip(data.RangeId * data.RangeSize)
                .Take(data.RangeSize)
                .ToArrayAsync();
        }
    }
}
