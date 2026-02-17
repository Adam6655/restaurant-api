using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDTOs
{
    public class clsCategoryDTO
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImageUrl { get; set; }
        public bool IsActive { get; set; }
        public clsCategoryDTO(int categoryID, string categoryName, string categoryImageUrl, bool isActive)
        {
            CategoryID = categoryID;
            CategoryName = categoryName;
            CategoryImageUrl = categoryImageUrl;
            IsActive = isActive;
        }
    }
}
