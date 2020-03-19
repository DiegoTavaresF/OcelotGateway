using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserAuthViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login(string userName, string password)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "user1",
                Email = "teste@teste.com"
            };

            return Ok(GenerateToken(user));
        }

        private UserAuthViewModel GenerateToken(User user)
        {
            if (user == null)
                throw new Exception();

            // get from appsettings.json
            var mySecuritySettings = new
            {
                Secret = "P8m<*z}*Y?duu5Bs8y;gup6]&6~%cn~`u7gNaf526$~z_zbDPtkRF*}cTJ{3",
                ExpirationMinutes = 30,
                Issuer = "SampleIssuer",
                ValideIn = "https://localhost"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(mySecuritySettings.Secret);

            var claims = new ClaimsIdentity() { };
            claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            claims.AddClaim(new Claim("UserId", user.Id.ToString()));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Issuer = mySecuritySettings.Issuer,
                Audience = mySecuritySettings.ValideIn,
                Expires = DateTime.UtcNow.AddMinutes(mySecuritySettings.ExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            return new UserAuthViewModel
            {
                Token = jwtToken,
                TokenType = "bearer",
                TokenExpiration = tokenDescriptor.Expires,
                UserName = user.Name,
                UserId = user.Id.ToString()
            };
        }
    }
}