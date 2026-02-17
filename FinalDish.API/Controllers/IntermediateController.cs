

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
    }
}
