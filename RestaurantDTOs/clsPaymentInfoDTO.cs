using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDTOs
{
    public class clsPaymentInfoDTO
    {
        public int OrderID { get; set; }
        public List<byte> PaymentMethod { get; set; }
        public List<decimal?> Amount { get; set; }
        public clsPaymentInfoDTO(int orderID, List<byte> paymentMethod, List<decimal?> amount)
        {
            OrderID = orderID;
            PaymentMethod = paymentMethod;
            Amount = amount;
        }
    }
}
