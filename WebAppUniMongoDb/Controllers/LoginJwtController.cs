using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAppUniMongoDb.Model;

namespace WebAppUniMongoDb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginJwtController : Controller
    {
        private JwtSettings jwtSettings;

        public LoginJwtController(JwtSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
        }

        // POST api/<LoginJwtController>
        [HttpPost]
        public IActionResult Post([FromBody] Credentials credentials)
        {
            if (credentials.Username.ToLower() == "alice" && credentials.Password.ToLower() == "faoro")
            {
                var token = GenerateJwtToken(credentials.Username);
                return Ok(new { token });
            }
            else
                return Unauthorized();
        }

        private string GenerateJwtToken(string username)
        {
            var secretKey = jwtSettings.SecretKey;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim (ClaimTypes.Name, username)
                }),
                Expires = DateTime.Now.AddMinutes(jwtSettings.TokenExpirationMinutes),
                Issuer = jwtSettings.Issuer,
                Audience = jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}

