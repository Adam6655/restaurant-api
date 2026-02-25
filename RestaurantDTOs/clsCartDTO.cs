using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDTOs
{
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
    public class clsCartDTO
    {
        public clsProductDTO Product { get; set; }
        public byte Quantity { get; set; }
        public string? Notes { get; set; }
        public List<clsAddOnDTO> AddOns { get; set; }
        public clsCartDTO(clsProductDTO product, byte quantity, string? notes, List<clsAddOnDTO> addOns)
        {
            Product = product;
            Quantity = quantity;
            Notes = notes;
            AddOns = addOns;
        }
    }
}
