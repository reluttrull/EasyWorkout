using EasyWorkout.Contracts.Requests;
using EasyWorkout.Identity.Api.Data;
using EasyWorkout.Identity.Api.Model;
using EasyWorkout.Identity.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace EasyWorkout.Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("fixed")]
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
        public async Task<IActionResult> Login([FromBody] Contracts.Requests.LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName);
            
            if (user == null)
                return Unauthorized(new { message = "Invalid Username" });

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
                return Unauthorized(new { message = "Invalid Password" });

            string accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken.Token;
            user.RefreshTokenExpiry = refreshToken.Expires;

            await _context.SaveChangesAsync();

            return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken.Token });
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid Refresh Token" });
            }
            if (user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return Unauthorized(new { Message = "Refresh Token Expired" });
            }
            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken.Token;
            user.RefreshTokenExpiry = newRefreshToken.Expires;
            await _context.SaveChangesAsync();

            return Ok(
                new 
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken.Token
                });
        }

        [HttpDelete("revoke/{refreshToken}")]
        public async Task<IActionResult> RevokeToken(string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user is null) return NotFound($"User with specified token not found.");

            user.RefreshToken = string.Empty;
            user.RefreshTokenExpiry = new DateTime();

            await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "RefreshToken");
            
            await _context.SaveChangesAsync();
            return Ok("Refresh token revoked.");
        }
    }
}
