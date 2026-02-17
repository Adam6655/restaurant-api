using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDTOs
{
    public class clsOrderStatusDTO
    {
        public int OrderID { get; set; }
        public byte OrderStatus { get; set; }
        public DateTime StatusDate { get; set; }
        public clsOrderStatusDTO(int orderID, byte orderStatus, DateTime statusDate)
        {
            OrderID = orderID;
            OrderStatus = orderStatus;
            StatusDate = statusDate;
        }
    }
}
