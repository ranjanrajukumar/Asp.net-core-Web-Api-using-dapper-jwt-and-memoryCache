using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPiWithDapper.Entities;
using WebAPiWithDapper.IRepo;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace WebAPiWithDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepo _users;
        public IConfiguration _configuration;

        public UsersController(IUsersRepo users, IConfiguration configuration)
        {
            _users = users;
            _configuration = configuration;
        }


        [HttpPost]
        public async Task<IActionResult> Post(Users1 user)
        {
            if (user != null && user.user1 != null && user.password != null)
                {
               // var userData = await GetUser(user.user1, user.password);
              
                var Jwt = _configuration.GetSection("Jwt").Get<Jwt>();
                if (user != null)
                {
                    var claims = new[]
                    {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub,Jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString())
                //new Claim("user1",user.user1),
                //new Claim("password",user.password)
                };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt.key));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        Jwt.Issuer,
                        Jwt.Audience,
                        claims,
                        expires: DateTime.Now.AddMinutes(1),
                        signingCredentials: signIn
                        );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid Credentials");
                }
            }
            else
            {
                return BadRequest("Invalid Credentials");
            }

        }

        [HttpGet]
        public async Task<IEnumerable<Users1>> GetUser(string users, string password)
        {
            //return await _users.GetUserData((string)users, (string)password);
            return await _users.GetUserData(users,password);
        }
    }
}
