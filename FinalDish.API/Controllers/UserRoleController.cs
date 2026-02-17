
using FinalDish.API.Constants;
using FinalDish.API.DTO;
using FinalDish.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalDish.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserRoleController : IntermediateController
    {
        private readonly UserManager<AppUser> userManager;

        public UserRoleController
            (UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        //  TODO: cache
        public async Task<IActionResult> GetUserRoles(string userName) 
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user is null)
                return Error500("User not found");

            return JsonResponse(await userManager.GetRolesAsync(user));
        }

        [HttpPost]
        [ResponseCache(CacheProfileName = CacheProfilesNames.NoStore)]
        public async Task<IActionResult> AddUserRole(UserRoleDTO data)
        {
            if (!RolesNames.IsValidRole(data.RoleName))
                return Error500("Unknown role.");

            var user = await userManager.FindByNameAsync(data.UserName);

            if (user is null)
                return Error500("User not found");

            var roleResult = await userManager.AddToRoleAsync(user, data.RoleName);

            return roleResult.Succeeded ?
                Ok($"Role {data.RoleName} has been successfully assigned to user {data.UserName}") :
                Error500(roleResult);
        }
    }
}
