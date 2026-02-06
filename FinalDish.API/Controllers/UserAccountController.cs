
using FinalDish.API.DTO;
using FinalDish.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FinalDish.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ConfigurationManager configuration;
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;

        public UserAccountController
            (ApplicationDbContext context,
            ConfigurationManager configuration,
            SignInManager<AppUser> signInManager, 
            UserManager<AppUser> userManager)
        {
            this.context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationDTO data) 
        {
            var user = new AppUser 
            {
                UserName = data.Name,
                Email = data.Email
            };

            var result = await userManager.CreateAsync(user, data.Password);

            if (result.Succeeded)
                return StatusCode(StatusCodes.Status201Created, $"User {user.UserName} has been created.");
            else
                return Problem(string.Join(" ", result.Errors), null, StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        public async Task<IActionResult> Login(RegistrationDTO data)
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
    }
}
