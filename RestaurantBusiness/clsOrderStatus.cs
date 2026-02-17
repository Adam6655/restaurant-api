using System;
using System.Data;
using RestaurantData;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace RestaurantBusiness
{
    public class clsOrderStatus
    {// I guess we wont need this class since will not include any method and wont add or update any transaction
        public int OrderID { get; set; }
        public byte OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        //public enum enOrderStatus { OrderPlaced = 1, Preparing = 2, ReadyToPick = 3, ReadyToDeliver = 4, OnTheWay = 5, Arrived = 6, Completed = 7 };
        private clsOrderStatus(int orderID, byte Orderstatus, DateTime orderDate)
        {
            OrderID = orderID;
            OrderStatus = Orderstatus;
            OrderDate = orderDate;
        }
    }
    //public class clsOrderStatusDTO
    //{
    //    public int OrderID { get; set; }
    //    public byte OrderStatus { get; set; }
    //    public DateTime OrderDate { get; set; }
    //    public clsOrderStatusDTO(int orderID, byte Orderstatus, DateTime orderDate)
    //    {
    //        OrderID = orderID;
    //        OrderStatus = Orderstatus;
    //        OrderDate = orderDate;
    //    }
    //}
}
