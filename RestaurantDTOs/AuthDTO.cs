using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDTOs
{
    public class clsLoginRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public clsLoginRequestDTO(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }
    }
    public class clsLogoutRequestDTO
    {
        public string Email { get; set; }
        public string RefreshToken { get; set; }
        public clsLogoutRequestDTO(string email, string refreshToken)
        {
            this.Email = email;
            this.RefreshToken = refreshToken;
        }
    }
    public class clsRefreshTokenRequestDTO
    {
        public string Email { get; set; }
        public string RefreshToken { get; set; }
        public clsRefreshTokenRequestDTO(string email, string refreshToken)
        {
            this.Email = email;
            this.RefreshToken = refreshToken;
        }
    }
    public class clsTokenResponseDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public clsTokenResponseDTO(string accessToken, string refreshToken)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
        }
    }
}
