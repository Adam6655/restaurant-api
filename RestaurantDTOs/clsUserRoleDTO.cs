using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDTOs
{
    public class clsUserRoleDTO
    {
        public int UserRoleID { get; set; }
        public byte UserRole { get; set; }
        public int UserID { get; set; }
        public clsUserRoleDTO(int userRoleID, byte userRole, int userID)
        {
            UserRoleID = userRoleID;
            UserRole = userRole;
            UserID = userID;
        }
    }
    public class clsUserRoleWithNameDTO
    {
        public int UserRoleID { get; set; }
        public byte UserRole { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public clsUserRoleWithNameDTO(int userRoleID, byte userRole, int userID, string userName)
        {
            UserRoleID = userRoleID;
            UserRole = userRole;
            UserID = userID;
            UserName = userName;
        }
    }
}
