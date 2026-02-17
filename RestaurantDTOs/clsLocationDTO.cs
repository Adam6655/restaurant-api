using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDTOs
{
    public class clsLocationDTO
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int UserID { get; set; }
        public bool IsActive { get; set; }
        public clsLocationDTO(int LocationiD, string locationName, string locationAddress, decimal latitude, decimal longitude, int userID, bool isActive)
        {
            LocationID = LocationiD;
            LocationName = locationName;
            LocationAddress = locationAddress;
            Latitude = latitude;
            Longitude = longitude;
            UserID = userID;
            IsActive = isActive;
        }
    }
}
