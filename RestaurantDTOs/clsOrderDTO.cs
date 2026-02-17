using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDTOs
{
    public class clsOrderDTO
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public byte ServiceType { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? DeliveryFee { get; set; }
        public int? LocationID { get; set; }
        public int UserID { get; set; }
        public int? DriverID { get; set; }
        public bool IsComplete { get; set; }
        public clsOrderDTO(int OrderiD, DateTime orderDate, byte serviceType, decimal totalAmount, decimal? deliveryFee, int? locationID, int userID, int? driverID, bool isComplete)
        {
            OrderID = OrderiD;
            OrderDate = orderDate;
            ServiceType = serviceType;
            TotalAmount = totalAmount;
            DeliveryFee = deliveryFee;
            LocationID = locationID;
            UserID = userID;
            DriverID = driverID;
            IsComplete = isComplete;
        }
    }
    public class clsProcessOrderDTO
    {
        public clsOrderDTO OrderDTO { get; set; }
        public clsPaymentInfoDTO PaymentInfoDTO { get; set; }
        public List<clsCheckoutCartItemsDTO> CheckoutCartItemsDTO { get; set; }
        public clsProcessOrderDTO(clsOrderDTO orderDTO, clsPaymentInfoDTO paymentInfoDTO, List<clsCheckoutCartItemsDTO> checkoutCartItemsDTO)
        {
            this.OrderDTO = orderDTO;
            this.PaymentInfoDTO = paymentInfoDTO;
            this.CheckoutCartItemsDTO = checkoutCartItemsDTO;
        }
    }
    public class clsOrderFullDetailsDTO
    {
        public clsOrderDTO OrderDTO { get; set; }
        public clsPaymentInfoDTO PaymentInfoDTO { get; set; }
        public clsCart2DTO Cart2 { get; set; }
        public clsOrderFullDetailsDTO(clsOrderDTO orderDTO, clsPaymentInfoDTO paymentInfoDTO, clsCart2DTO cart2)
        {
            this.OrderDTO = orderDTO;
            this.PaymentInfoDTO = paymentInfoDTO;
            this.Cart2 = cart2;
        }
    }
}
