using RestaurantData;
using RestaurantDTOs;
using System;
using System.Data;
using System.Drawing;
using System.Linq.Expressions;
using System.Numerics;
using static RestaurantBusiness.clsUserRole;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace RestaurantBusiness
{
    public class clsUser
    {
        public int UserID { get; private set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public int Coins { get; set; }
        public string DeviceToken { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public string? RefreshTokenHash { get; set; }
        public clsUserDTO UserDTO
        {
            get { return (new clsUserDTO(this.UserID, this.UserName, this.DateCreated, this.Coins, 
                this.DeviceToken, this.Email,this.PasswordHash,this.Phone,this.RefreshTokenHash)); }
        }
        public clsCoinSavingsSummaryDTO CoinSavingsSummaryDTO
        {
            get {
                return (new clsCoinSavingsSummaryDTO(GetUserCoinsTransactionsHistory(this.UserID),this.Coins,
                  GetCoinsValue(this.Coins), GetSavedAmountFromCoins()));
            }
        }
        private clsUser(int userID, string userName, DateTime dateCreated, int coins, string deviceToken, string email, string passwordHash, string phone, string refreshTokenHash)
        {
            this.UserID = userID;
            this.UserName = userName;
            this.DateCreated = dateCreated;
            this.Coins = coins;
            this.DeviceToken = deviceToken;
            this.Email = email;
            this.PasswordHash = passwordHash;
            this.Phone = phone;
            this.RefreshTokenHash = refreshTokenHash;
        }
        public clsUser()
        {
            this.UserID = -1;
            this.UserName = "";
            this.DateCreated = DateTime.Now;
            this.Coins = 0;
            this.DeviceToken = "";
            this.Email = "";
            this.PasswordHash = "";
            this.Phone = "";
            this.RefreshTokenHash = null;
        }
        public static clsUser Find(int ID)
        {
            clsUserDTO UserDTO = clsUsersData.GetUserByID(ID);

            if (UserDTO != null)
            {
                return new clsUser(ID, UserDTO.UserName, UserDTO.DateCreated, UserDTO.Coins, UserDTO.DeviceToken, UserDTO.Email, UserDTO.PasswordHash, UserDTO.Phone, UserDTO.RefreshTokenHash);
            }
            else
                return null;
        }
        public static clsUser Find(string Email, string PasswordHash)
        {
            clsUserDTO UserDTO = clsUsersData.GetUser(Email, PasswordHash);

            if (UserDTO != null)
            {
                return new clsUser(UserDTO.UserID, Email, UserDTO.DateCreated, UserDTO.Coins, UserDTO.DeviceToken, UserDTO.Email, PasswordHash, UserDTO.Phone, UserDTO.RefreshTokenHash);
            }
            else
                return null;
        }
        public static clsUser Find(string email)
        {
            clsUserDTO UserDTO = clsUsersData.GetUser(email);
            if (UserDTO != null)
            {
                return new clsUser(UserDTO.UserID, UserDTO.UserName, UserDTO.DateCreated, UserDTO.Coins, UserDTO.DeviceToken, email, UserDTO.PasswordHash, UserDTO.Phone, UserDTO.RefreshTokenHash);
            }
            else
                return null;
        }
        private bool _AddNewUser()
        {
            this.UserID = clsUsersData.AddUser(this.UserDTO);

            return (this.UserID != -1);
        }
        private bool _UpdateUser()
        {
            return clsUsersData.UpdateUser(this.UserDTO);
        }
        public bool Save()
        {
            if (this.UserID == -1)
            {
                return _AddNewUser();
            }
            else
            {
                return _UpdateUser();
            }
        }
        public static int GetCoinsValue(int Coins)
        {
            return clsCoinsTransactionsData.GetCoinsValue(Coins);
        }
        public int GetCoinsValue()
        {
            return GetCoinsValue(this.Coins);
        }
        public static List<clsCoinTransactionDTO> GetUserCoinsTransactionsHistory(int UserID)
        {
            return clsCoinsTransactionsData.GetUserCoinsTransactionsHistory(UserID);
        }
        public List<clsCoinTransactionDTO> GetUserCoinsTransactionsHistory()
        {
            return GetUserCoinsTransactionsHistory(this.UserID);
        }
        public static List<clsOrderDTO> GetAllUserOrders(int UserID)
        {
            return clsOrdersData.GetAllUserOrders(UserID);
        }
        public int? GetLastOrderID()
        {
            return GetUserLastOrderID(this.UserID);
        }
        public static int? GetUserLastOrderID(int UserID)
        {
            return clsOrdersData.GetUserLastOrderID(UserID);
        }
        public decimal GetSavedAmountFromCoins()
        {
            return clsCoinsTransactionsData.GetSavedAmountFromCoins(this.UserID);
        }
        public string GetUserRoleText()
        {
            int? Role = GetUserRole();
            enUserRole? UserRole = Role == null? null : (enUserRole)Role;

            switch (UserRole)
            {
                case enUserRole.Admin:
                    return "Admin";

                case enUserRole.Staff:
                    return "Staff";

                case enUserRole.Driver:
                    return "Driver";

                default:
                    return "Customer";
            }
        }
        public int? GetUserRole()
        {
            return GetUserRole(this.UserID);
        }
        public static int? GetUserRole(int UserID)
        {
            return clsUserRolesData.GetUsersRole(UserID);
        }
        public static List<clsOrderDTO> GetDriverDeliveryOrders(int DriverID)
        {
            return clsOrdersData.GetDriverDeliveryOrders(DriverID);
        }
        public clsCoinSavingsSummaryDTO GetUserCoinSavingsSummary()
        {
            clsCoinSavingsSummaryDTO CoinSavingsSummaryDTO = new clsCoinSavingsSummaryDTO(
                clsCoinsTransactionsData.GetUserCoinsTransactionsHistory(this.UserID),this.Coins,
                clsCoinsTransactionsData.GetCoinsValue(this.Coins),
                clsCoinsTransactionsData.GetSavedAmountFromCoins(this.UserID));

            return CoinSavingsSummaryDTO;
        }
    }
}