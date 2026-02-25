using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestaurantBusiness;
using RestaurantDTOs;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static RestaurantBusiness.clsUserRole;

namespace RestaurantApi.Controllers
{
    [ApiController]
    [Route("api/Auth")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        // This endpoint handles user login.
        // It verifies credentials and returns:
        // - AccessToken (JWT) for calling secured APIs
        // - RefreshToken for renewing the access token later
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                clsUser user = clsUser.Find(request.Email);

                if (user == null)
                    return Unauthorized("Invalid credentials");

                bool isValidPassword =
                    BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

                if (!isValidPassword)
                    return Unauthorized("Invalid credentials");

                var accessToken = GenerateAccessToken(user);

                var refreshToken = GenerateRefreshToken();

                user.RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(refreshToken);
                user.Save();

                return Ok(new clsTokenResponseDTO
                (
                    accessToken,
                    refreshToken
                )
                );
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        private static string GenerateRefreshToken()
        {
            var bytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
        private static string GenerateAccessToken(clsUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),

                new Claim(ClaimTypes.Email, user.Email),

                new Claim(ClaimTypes.Role, user.GetUserRoleText())
            };

            var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "Fast-Food-Api",
                audience: "Fast-Food-Api-Users",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        //refresh endpint
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] clsRefreshTokenRequestDTO request)
        {
            try
            {
                clsUser user = clsUser.Find(request.Email);

                if (user == null)
                    return Unauthorized("Invalid refresh request");

                if (user.RefreshTokenHash == null)
                    return Unauthorized("Refresh token is revoked");

                bool refreshValid = BCrypt.Net.BCrypt.Verify(request.RefreshToken, user.RefreshTokenHash);
                if (!refreshValid)
                    return Unauthorized("Invalid refresh token");

                string newAccessToken = GenerateAccessToken(user);

                string newRefreshToken = GenerateRefreshToken();
                user.RefreshTokenHash = BCrypt.Net.BCrypt.HashPassword(newRefreshToken);
                user.Save();

                return Ok(new clsTokenResponseDTO
                (
                    newAccessToken,
                    newRefreshToken
                )
                );
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] clsLogoutRequestDTO request)
        {
            try
            {
                clsUser user = clsUser.Find(request.Email);

                if (user.RefreshTokenHash == null)
                {
                    return Ok();
                }

                bool refreshValid = BCrypt.Net.BCrypt.Verify(request.RefreshToken, user.RefreshTokenHash);
                if (!refreshValid || user == null)
                    return Ok();

                user.RefreshTokenHash = null;
                user.Save();
                return Ok("Logged out successfully");
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        
        [HttpPost("Signup")]
        public IActionResult Signup([FromBody] clsUser user)
        {
            try
            {
                user.RefreshTokenHash = GenerateRefreshToken();
                user.Save();

                var accessToken = GenerateAccessToken(user);

                return Ok(new clsTokenResponseDTO
                (
                accessToken,
                user.RefreshTokenHash
                ));
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex,"This email already exists");
            }
        }
    }
}