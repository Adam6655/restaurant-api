using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDTOs
{
    public class clsUserDTO
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public int Coins { get; set; }
        public string DeviceToken { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public clsUserDTO(int userID, string userName, DateTime dateCreated, int coins, string deviceToken, string email, string passwordHash, string phone)
        {
            UserID = userID;
            UserName = userName;
            DateCreated = dateCreated;
            Coins = coins;
            DeviceToken = deviceToken;
            Email = email;
            PasswordHash = passwordHash;
            Phone = phone;
        }
    }
    public class clsLoginRequestDTO
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public clsLoginRequestDTO(string userName, string passwordHash)
        {
            UserName = userName;
            PasswordHash = passwordHash;
        }
    }
}
