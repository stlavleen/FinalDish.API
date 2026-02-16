

using FinalDish.API.Constants;
using FinalDish.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalDish.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [HttpPost]
        [ResponseCache(CacheProfileName = CacheProfilesNames.NoStore)]
        public async Task<IActionResult> AddRoles(string[] roles)
        {
            var succeededCount = 0;
            var errors = new List<string>(roles.Length);

            foreach (var roleName in roles)
            {
                if (!RolesNames.Content.Contains(roleName))
                {
                    errors.Add("Unknown role.");
                    continue;
                }

                var role = new IdentityRole(roleName);
                var result = await roleManager.CreateAsync(role);

                if (result.Succeeded)
                    succeededCount++;
                else
                    errors.Add(string.Join(" ", result.Errors.Select(x => x.Description)));
            }

            return succeededCount == roles.Length ?
                StatusCode(StatusCodes.Status200OK, $"{succeededCount} roles have been successfully added.") :
                StatusCode(StatusCodes.Status500InternalServerError,
                $"{roles.Length - succeededCount} items failed. Errors: {string.Join("\n", errors)}");
        }

        [HttpDelete]
        [ResponseCache(CacheProfileName = CacheProfilesNames.NoStore)]
        public async Task<IActionResult> RemoveRole(string roleName)
        {
            if (!RolesNames.Content.Contains(roleName))
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: Unknown role.");

            var role = await roleManager.FindByNameAsync(roleName);

            if (role is null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error: Role is not found.");

            var result = await roleManager.DeleteAsync(role);

            return result.Succeeded ?
                StatusCode(StatusCodes.Status200OK, $"Role has been successfully removed.") :
                StatusCode(StatusCodes.Status500InternalServerError,
                $"Error: {string.Join("\n", result.Errors.Select(x => x.Description))}");
        }
    }
}
