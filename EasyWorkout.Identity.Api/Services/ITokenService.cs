using EasyWorkout.Identity.Api.Model;

namespace EasyWorkout.Identity.Api.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        (string Token, DateTime Expires) GenerateRefreshToken();
    }
}
