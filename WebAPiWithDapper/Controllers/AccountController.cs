using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WebAPiWithDapper.Entities;
using WebAPiWithDapper.IRepo;

namespace WebAPiWithDapper.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtSettings jwtSettings;
        private readonly IUsersRepo _users;
        private const string userListCacheKey = "UserList";
        private IMemoryCache _cache;
        private ILogger<AccountController> _logger;
        public AccountController(JwtSettings jwtSettings, IUsersRepo users, IMemoryCache cache,
        ILogger<AccountController> logger)
        {
            this.jwtSettings = jwtSettings;
            _users = users;
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private IEnumerable<Users> logins = new List<Users>()
        {

           //var usersData =   _users.GetUserData();
        new Users()
            {

                Id = Guid.NewGuid(),
                EmailId = "adminakp@gmail.com",
                UserName ="Admin",
                Password="Admin",
            },
            new Users()
            {
                Id = Guid.NewGuid(),
                EmailId = "adminakp@gmail.com",
                UserName ="User1",
                Password="Admin",
            }
        };

        /// <summary>
        /// Generate an Access token
        /// </summary>
        /// <param name="userLogins"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetToken(UserLogins userLogins)
        {
            try
            {
                _logger.Log(LogLevel.Information, "Trying to fetch the list of employees from cache.");
                if (_cache.TryGetValue(userLogins.UserName, out UserTokens Token))
                {
                    _logger.Log(LogLevel.Information, "User list found in cache.");
                }
                else
                {
                    var userData = await _users.GetUserData();
                    //var Token = new UserTokens();
                    var Valid = userData.Any(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase));
                    if (Valid)
                    {
                        var user = userData.FirstOrDefault(x => x.UserName.Equals(userLogins.UserName, StringComparison.OrdinalIgnoreCase));
                        Token = JwtHelpers.JwtHelpers.GenTokenkey(new UserTokens()
                        {
                            EmailId = user.EmailId,
                            GuidId = Guid.NewGuid(),
                            UserName = user.UserName,
                            Id = user.Id,

                        }, jwtSettings);

                        _logger.Log(LogLevel.Information, "Employee list not found in cache. Fetching from database.");

                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                                .SetPriority(CacheItemPriority.Normal)
                                .SetSize(1024);

                        _cache.Set(userLogins.UserName, Token, cacheEntryOptions);
                    }
                    else
                    {
                        return BadRequest($"wrong password");
                    }
                }
                return Ok(Token);
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// Get List of UserAccounts   
        /// </summary>
        /// <returns>List Of UserAccounts</returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetList()
        {
            return Ok(logins);
        }
    }
}
