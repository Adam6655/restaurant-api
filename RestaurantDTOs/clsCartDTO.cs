using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDTOs
{
    //public class clsCartDTO
    //{
    //    public int ProductName { get; set; }
    //    public string ProductDescription { get; set; }
    //    public byte Quantity { get; set; }
    //    public decimal Price { get; set; }
    //    public string ImageURL { get; set; }
    //    public int Calories { get; set; }
    //    public string? Notes { get; set; }
    //    public clsCartDTO(int productName, string productDescription, byte quantity, decimal price, string imageURL, int calories, string? notes)
    //    {
    //        ProductName = productName;
    //        ProductDescription = productDescription;
    //        Quantity = quantity;
    //        Price = price;
    //        ImageURL = imageURL;
    //        Calories = calories;
    //        Notes = notes;
    //    }
    //}
    public class clsCheckoutCartItemsDTO
    {
        public int ProductID { get; set; }
        public byte Quantity { get; set; }
        public decimal Price { get; set; }
        public string? Notes { get; set; }
        public clsCheckoutCartItemsDTO(int productID, byte quantity, decimal price, string? notes)
        {
            ProductID = productID;
            Quantity = quantity;
            Price = price;
            Notes = notes;
        }
    }
    public class clsCart2DTO
    {
        public clsProductDTO Product { get; set; }
        public byte Quantity { get; set; }
        public string? Notes { get; set; }
        public List<clsAddOnDTO> AddOns { get; set; }
        public clsCart2DTO(clsProductDTO product, byte quantity, string? notes, List<clsAddOnDTO> addOns)
        {
            Product = product;
            Quantity = quantity;
            Notes = notes;
            AddOns = addOns;
        }
    }
}
