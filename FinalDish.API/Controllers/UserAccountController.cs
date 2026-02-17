
using FinalDish.API.Constants;
using FinalDish.API.DTO;
using FinalDish.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FinalDish.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserAccountController : IntermediateController
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
            var user = new AppUser
            {
                UserName = data.Name,
                Email = data.Email
            };

            var userResult = await userManager.CreateAsync(user, data.Password);

            return userResult.Succeeded ?
                Created(string.Empty, $"User {user.UserName} has been created.") :
                Error500(userResult);
        }

        [HttpPost]
        [ResponseCache(CacheProfileName = CacheProfilesNames.NoStore)]
        public async Task<IActionResult> Login(LoginDTO data)
        {
            var user = await userManager.FindByNameAsync(data.Name);

            if (user is null)
                return Error500($"User with name = {data.Name} does not exist");

            var passwordIsCorrect = await userManager.CheckPasswordAsync(user, data.Password);

            if (!passwordIsCorrect)
                return BadRequest("Login is failed. Check login and password");

            var jwtString = await CreateJWTAsync(user);

            return Ok(jwtString);
        }

        private async Task<string> CreateJWTAsync(AppUser user) 
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var roles = await userManager.GetRolesAsync(user);
            var claims = CreateClaims(roles, user);
            var jwt = new JwtSecurityToken(
                configuration["JWT:Issuer"],
                configuration["JWT:Audience"],
                claims,
                null,
                DateTime.Now.AddSeconds(300),
                credentials);

            var jwtString = new JwtSecurityTokenHandler().WriteToken(jwt);

            return jwtString;
        }

        private IEnumerable<Claim> CreateClaims(IList<string> roles, AppUser user) 
        {
            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };
            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            return claims;
        }
    }
}
