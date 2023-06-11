/*using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using Asp.Versioning;
using DAL.EF.App;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.v1.Error;
using Public.DTO.v1.Identity;
using PublicApi.DTO.v1.Identity;
using WebApp.Extensions;
using AppRole = Domain.App.Identity.AppRole;
using AppUser = Domain.App.Identity.AppUser;


namespace WebApp.ApiControllers.Identity
{
    /// <summary>
    /// User Management Api Controller
    /// </summary>
    [ApiController]
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
      private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly JwtSecurityTokenHandler _tokenHandler = new();
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _ctx;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="context"></param>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public AccountController(ILogger<AccountController> logger, IConfiguration configuration, ApplicationDbContext context,
                                 SignInManager<AppUser> signInManager, 
                                 UserManager<AppUser> userManager, 
                                 RoleManager<AppRole> roleManager
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _ctx = context;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Login with provided data
        /// </summary>
        /// <param name="dto">login data</param>
        /// <returns>first and last names of user and JWT for authorization</returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login dto)
        {
            var appUser = await _userManager.FindByEmailAsync(dto.Email);
            
            Thread.Sleep(new Random().Next(100, 1000));
            
            if (appUser == null)
            {
                _logger.LogWarning("WebApi login failed. User {User} not found", dto.Email);
                return NotFound(new Message("User/Password problem!"));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(appUser, dto.Password, false);
            if (result.Succeeded)
            {
                
                var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
                var jwt = IdentityExtensions.GenerateJwt(
                    claimsPrincipal.Claims,
                    _configuration["JWT:Key"],                    
                    _configuration["JWT:Issuer"],
                    _configuration["JWT:A"],
                    DateTime.Now.AddDays(_configuration.GetValue<int>("JWT:ExpireDays"))
                    );
                _logger.LogInformation("WebApi login. User {User}", dto.Email);
                return Ok(new JwtResponse
                {
                    Token = jwt,
                    Firstname = appUser.Firstname,
                    Lastname = appUser.Lastname,
                    Roles = _userManager.GetRolesAsync(appUser)?.Result ?? new Collection<string>()
                });
            }
            
            _logger.LogWarning("WebApi login failed. User {User} - bad password", dto.Email);
            return NotFound(new Message("User/Password problem!"));
        }
        
        /// <summary>
        /// Register new user.
        /// </summary>
        /// <param name="dto">new user data</param>
        /// <returns>first and last names of created user and JWT for authorization</returns>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Register dto)
        {
            var appUser = await _userManager.FindByEmailAsync(dto.Email);
            if (appUser != null)
            {
                _logger.LogWarning(" User {User} already registered", dto.Email);
                return BadRequest(new Message("User already registered"));
            }

            appUser = new Domain.App.Identity.AppUser()
            {
                Email = dto.Email,
                UserName = dto.Email,
                Firstname = dto.Firstname,
                Lastname = dto.Lastname,
            };
            var result = await _userManager.CreateAsync(appUser, dto.Password);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("User {Email} created a new account with password", appUser.Email);
                
                var user = await _userManager.FindByEmailAsync(appUser.Email);
                if (user != null)
                {                
                    var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
                    var jwt = IdentityExtensions.GenerateJwt(
                        claimsPrincipal.Claims,
                        _configuration["JWT:Key"],                    
                        _configuration["JWT:Issuer"],
                        _configuration["JWT:Issuer"],
                        DateTime.Now.AddDays(_configuration.GetValue<int>("JWT:ExpireDays"))
                    );
                    _logger.LogInformation("WebApi login. User {User}", dto.Email);
                    return Ok(new JwtResponse()
                    {
                        Token = jwt,
                        Firstname = appUser.Firstname,
                        Lastname = appUser.Lastname,
                        Roles = _userManager.GetRolesAsync(user)?.Result ?? new Collection<string>()
                    });
                    
                }

                _logger.LogInformation("User {Email} not found after creation", appUser.Email);
                return BadRequest(new Message("User not found after creation!"));
            }
            
            var errors = result.Errors.Select(error => error.Description).ToList();
            return BadRequest(new Message {Messages = errors});
        }


        [NonAction]
        private async Task<string> GenerateJwtForUser(AppUser user)
        {
            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            var jwt = IdentityExtensions.GenerateJwt(
                claimsPrincipal.Claims,
                _configuration["JWT:Key"],
                _configuration["JWT:Issuer"],
                _configuration["JWT:Issuer"],
                DateTime.Now.AddDays(_configuration.GetValue<int>("JWT:ExpireDays"))
            );

            return jwt;
        }
    }
}*/