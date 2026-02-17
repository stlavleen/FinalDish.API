
using FinalDish.API.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalDish.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController : IntermediateController
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [HttpGet]
        //  TODO: cache
        public async Task<IdentityRole[]> GetRoles() 
        {
            return await roleManager.Roles.ToArrayAsync();
        }

        [HttpPost]
        [ResponseCache(CacheProfileName = CacheProfilesNames.NoStore)]
        public async Task<IActionResult> AddRole(string name)
        {
            if (!RolesNames.Content.Contains(name))
                return Error500("Error: Unknown role");

            var role = new IdentityRole(name);
            var result = await roleManager.CreateAsync(role);

            return result.Succeeded ?
                Created(string.Empty, $"{name} role has been successfully added.") :
                Error500(result);
        }

        [HttpDelete]
        [ResponseCache(CacheProfileName = CacheProfilesNames.NoStore)]
        public async Task<IActionResult> RemoveRole(string roleName)
        {
            if (!RolesNames.Content.Contains(roleName))
                return Error500("Error: Unknown role.");

            var role = await roleManager.FindByNameAsync(roleName);

            if (role is null)
                return Error500("Error: Role is not found.");

            var result = await roleManager.DeleteAsync(role);

            return result.Succeeded ?
                Ok("Role has been successfully removed.") :
                Error500(result);
        }
    }
}
