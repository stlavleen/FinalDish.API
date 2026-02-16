
using FinalDish.API.Constants;
using FinalDish.API.DTO;
using FinalDish.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FinalDish.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<AppUser> userManager;

        public UserAccountController
            (IConfiguration configuration,
            UserManager<AppUser> userManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
        }

        [HttpPost]
        [ResponseCache(CacheProfileName = CacheProfilesNames.NoStore)]
        public async Task<IActionResult> Register(RegistrationDTO data) 
        {
            if (data.Role is not null && !RolesNames.Content.Contains(data.Role))
                return Problem("User is not created. Unknown role.", null, StatusCodes.Status500InternalServerError);

            var user = new AppUser 
            {
                UserName = data.Name,
                Email = data.Email
            };

            var result = await userManager.CreateAsync(user, data.Password);

            if (result.Succeeded) 
            {
                result = await userManager.AddToRoleAsync(user, data.Role);

                if (result.Succeeded)
                    return StatusCode(StatusCodes.Status201Created, $"User {user.UserName} has been created.");
            }

            return Problem(JoinErrors(result), null, StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        [ResponseCache(CacheProfileName = CacheProfilesNames.NoStore)]
        public async Task<IActionResult> Login(LoginDTO data)
        {
            var user = await userManager.FindByNameAsync(data.Name);

            if (user is null)
                return Problem($"User with name = {data.Name} does not exist", null, StatusCodes.Status500InternalServerError);

            var passwordIsCorrect = await userManager.CheckPasswordAsync(user, data.Password);

            if (passwordIsCorrect) 
            {
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var claims = new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };
                var jwt = new JwtSecurityToken(
                    configuration["JWT:Issuer"], 
                    configuration["JWT:Audience"], 
                    claims, 
                    null, 
                    DateTime.Now.AddSeconds(300), 
                    credentials);

                var jwtString = new JwtSecurityTokenHandler().WriteToken(jwt);

                return StatusCode(StatusCodes.Status200OK, jwtString);
            }  
            else
                return Problem("Login is failed. Check login and password", null, StatusCodes.Status400BadRequest);
        }

        private string JoinErrors(IdentityResult result) => 
            string.Join(" ", result.Errors.Select(x => x.Description));
    }
}
