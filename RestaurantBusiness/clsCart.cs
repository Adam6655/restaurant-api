using System;
using System.Data;
using RestaurantData;

namespace RestaurantBusiness
{
    public class clsCart
    {// I guess we wont need this class since will not include any method and wont add or update any transaction
        // i guess this class will be implemented in the frontend
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public int OrderID { get; set; }
        public byte Quantity { get; set; }
        public decimal Price { get; set; }
        public string Notes { get; set; }
        private clsCart(int iD, int userID, int productID, int orderID, byte quantity, decimal price, string notes)
        {
            ID = iD;
            UserID = userID;
            ProductID = productID;
            OrderID = orderID;
            Quantity = quantity;
            Price = price;
            Notes = notes;
        }
    }
    //public class clsCartDTO
    //{
    //    public string Name { get; set; }
    //    public string Description { get; set; }
    //    public byte Quantity { get; set; }
    //    public decimal Price { get; set; }
    //    public string ImageUrl { get; set; }
    //    public int Calories { get; set; }
    //    public clsCartDTO(string name, string description, byte quantity, decimal price, string imageUrl, int calories)
    //    {
    //        Name = name;
    //        Description = description;
    //        Quantity = quantity;
    //        Price = price;
    //        ImageUrl = imageUrl;
    //        Calories = calories;
    //    }
    //}
}
