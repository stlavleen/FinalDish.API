

using FinalDish.API.Constants;
using FinalDish.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalDish.API.Controllers
{
    /// <summary>
    /// Use as intermediate class between custom 
    /// controllers and <see cref="ControllerBase"/> 
    /// </summary>
    public class IntermediateController : ControllerBase
    {
        protected ObjectResult Error500(IdentityResult result) =>
            Problem(JoinErrors(result), null, StatusCodes.Status500InternalServerError);

        protected ObjectResult Error500(string message) =>
            Problem(message, null, StatusCodes.Status500InternalServerError);

        protected JsonResult JsonResponse(object value) =>
            new JsonResult(value)
            {
                StatusCode = StatusCodes.Status200OK
            };

        protected string JoinErrors(IdentityResult result) =>
            string.Join(" ", result.Errors.Select(x => x.Description));

        protected bool IsAuthorizedRequest(AppUser? user)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return IsSupervisorRole(identity) || IsNameMatch(user, identity);
        }

        protected bool IsSupervisorRole(ClaimsIdentity? identity)
        {
            var roles = identity?.FindAll(ClaimTypes.Role)?.Select(x => x.Value);

            return roles is not null && roles.Count() > 0 && roles.Contains(RolesNames.Moderator);
        }

        protected bool IsNameMatch(AppUser? user, ClaimsIdentity? identity)
        {
            var userName = user?.UserName;
            var nameFromIdentity = identity?.FindFirst(ClaimTypes.Name)?.Value;

            return userName == nameFromIdentity && userName is not null;
        }
    }
}
