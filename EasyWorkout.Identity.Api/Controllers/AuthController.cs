using EasyWorkout.Contracts.Requests;
using EasyWorkout.Identity.Api.Data;
using EasyWorkout.Identity.Api.Mapping;
using EasyWorkout.Identity.Api.Model;
using EasyWorkout.Identity.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Sprache;
using System.Data;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EasyWorkout.Identity.Api.Controllers
{
    [ApiController]
    [EnableRateLimiting("fixed")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthController(
            UserManager<User> userManager, 
            ILogger<AuthController> logger,
            AppDbContext context, 
            ITokenService tokenService,
            IUserService userService)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost(Endpoints.Auth.Register)]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName);
                if (existingUser is not null)
                {
                    ModelState.AddModelError("errors", $"Username {request.UserName} is already taken.");
                    return BadRequest(ModelState);
                }

                User user = new()
                {
                    Email = request.Email,
                    UserName = request.UserName,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    JoinedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    LastEditedDate = DateTime.UtcNow
                };

                var createUserResult = await _userManager.CreateAsync(user, request.Password);

                if (!createUserResult.Succeeded)
                {
                    var errorsText = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to create user.  Errors: {e}", errorsText);
                    foreach (var error in createUserResult.Errors)
                    {
                        ModelState.AddModelError("errors", error.Description);
                    }
                    return BadRequest(ModelState);
                }

                var addClaimUserResult = await _userManager.AddClaimAsync(user, new Claim(AuthConstants.FreeMemberUserClaimName, "true"));

                if (!addClaimUserResult.Succeeded)
                {
                    var errorsText = string.Join(", ", addClaimUserResult.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to add claim to user.  Errors: {e}", errorsText);
                }

                _logger.LogInformation("User successfully created with id {id} and username {username}.", user.Id, user.UserName);
                return CreatedAtAction(nameof(Register), null);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost(Endpoints.Auth.Login)]
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

            _logger.LogDebug("User successfully logged in with id {id} and username {username}.", user.Id, user.UserName);
            return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken.Token });
        }


        [HttpPost(Endpoints.Auth.Refresh)]
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

        [HttpDelete(Endpoints.Auth.Revoke)]
        public async Task<IActionResult> RevokeToken(string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user is null) return NotFound($"User with specified token not found.");

            user.RefreshToken = string.Empty;
            user.RefreshTokenExpiry = new DateTime();

            await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "RefreshToken");
            
            await _context.SaveChangesAsync();
            _logger.LogDebug("User successfully logged out with id {id} and username {username}.", user.Id, user.UserName);
            return Ok("Refresh token revoked.");
        }

        [HttpGet(Endpoints.Auth.Get)]
        public async Task<IActionResult> Get(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var user = await _userService.GetByIdAsync(userId!.Value, token);
            if (user is null) return NotFound($"User with id {userId!.Value} not found.");

            return Ok(user.MapToResponse());
        }

        [HttpPut(Endpoints.Auth.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var user = await _userService.UpdateAsync(userId!.Value, request, token);
            if (user is null) return NotFound();
            _logger.LogInformation("User with id {id} updated account info.", userId);
            return Ok(user.MapToResponse());
        }

        [HttpPut(Endpoints.Auth.ChangeEmail)]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var user = await _userService.ChangeEmailAsync(userId!.Value, request, token);
            if (user is null) return NotFound();
            _logger.LogInformation("User with id {id} changed email to {email}.", userId, request.Email);
            return Ok(user.MapToResponse());
        }

        [HttpPut(Endpoints.Auth.ChangePassword)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            if (userId is null)
            {
                _logger.LogWarning("Userid not found in context.");
                return BadRequest("User not found.");
            }

            var user = await _userService.ChangePasswordAsync(userId!.Value, request, token);
            if (user is null) return NotFound();
            _logger.LogInformation("User with id {id} changed password.", userId);
            return Ok(user.MapToResponse());
        }
    }
}
