using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDTOs
{
    public class clsProductAddOnDTO
    {
        public int ProductAddOnID {  get; set; }
        public int ProductID { get; set; }
        public int AddOnID { get; set; }
        public clsProductAddOnDTO(int productAddOnID, int productID, int addOnID)
        {
            ProductAddOnID = productAddOnID;
            ProductID = productID;
            AddOnID = addOnID;
        }
    }
    public class clsProductAddOnsRequestDTO
    {
        public int ProductID { get; set; }
        public List<int> AddOnIDs { get; set; }
        public clsProductAddOnsRequestDTO(int productID, List<int> addOnIDs)
        {
            ProductID = productID;
            AddOnIDs = addOnIDs;
        }
    }
}
