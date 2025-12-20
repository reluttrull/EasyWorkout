using EasyWorkout.Contracts.Requests;
using EasyWorkout.Identity.Api.Data;
using EasyWorkout.Identity.Api.Model;
using EasyWorkout.Identity.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace EasyWorkout.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(
            UserManager<User> userManager, 
            ILogger<AuthController> logger,
            AppDbContext context, 
            ITokenService tokenService)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request) // todo: make RegistrationRequest
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName);
                if (existingUser is not null) return BadRequest("User already exists.");

                // handle claims?

                User user = new()
                {
                    Email = request.Email,
                    UserName = request.UserName,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    JoinedDate = DateOnly.FromDateTime(DateTime.UtcNow)
                };

                var createUserResult = await _userManager.CreateAsync(user, request.Password);

                if (!createUserResult.Succeeded)
                {
                    var errorsText = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to create user.  Errors: {e}", errorsText);
                    return BadRequest($"Failed to create user.  Errors: {errorsText}");
                }

                // save user claims?
                var addClaimUserResult = await _userManager.AddClaimAsync(user, new Claim(AuthConstants.FreeMemberUserClaimName, "true"));

                if (!addClaimUserResult.Succeeded)
                {
                    var errorsText = string.Join(", ", addClaimUserResult.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to add claim to user.  Errors: {e}", errorsText);
                }

                return CreatedAtAction(nameof(Register), null);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName);
            // todo: deal with password!
            if (user == null)
                return Unauthorized(new { message = "Invalid Username or Password" });

            var accessToken = _tokenService.GenerateAccessToken(user);

            await _context.SaveChangesAsync();

            return Ok(accessToken);
        }
    }
}
