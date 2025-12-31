using EasyWorkout.Identity.Api.Model;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace EasyWorkout.Identity.Api.Services
{
    public class TokenService : ITokenService
    {
        private string tokenSecret = string.Empty;
        private static readonly TimeSpan tokenLifetime = TimeSpan.FromMinutes(15);
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config; 
            tokenSecret = Environment.GetEnvironmentVariable("TOKEN_SECRET") ?? _config.GetValue<string>("TOKEN_SECRET")!;
        }

        public string GenerateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(tokenSecret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Name, $"{user.FirstName} {user.LastName}"),
                new("userid", user.Id.ToString())
            };

            // for now, everyone is a free member
            claims.Add(new Claim(AuthConstants.FreeMemberUserClaimName, "true"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(tokenLifetime),
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? _config.GetValue<string>("JWT_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? _config.GetValue<string>("JWT_AUDIENCE"),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);
            return jwt;
        }


        (string Token, DateTime Expires) ITokenService.GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return (
                Token: Convert.ToBase64String(randomNumber),
                Expires: DateTime.UtcNow.AddDays(14)
            );
        }
    }
}
