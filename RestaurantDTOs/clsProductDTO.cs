using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDTOs
{
    public class clsProductDTO
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        public string ImageUrl { get; set; }
        public int Calories { get; set; }
        public bool IsActive { get; set; }
        public clsProductDTO(int productID, string productName, string productDescription, decimal price, int categoryID, string imageUrl, int calories, bool active)
        {
            ProductID = productID;
            ProductName = productName;
            ProductDescription = productDescription;
            Price = price;
            CategoryID = categoryID;
            ImageUrl = imageUrl;
            Calories = calories;
            IsActive = active;
        }
    }
    public class clsProductAddOnSelectionDTO
    {
        public int AddOnID { get; set; }
        public string ImageURL { get; set; }
        public bool IsSelected { get; set; }
        public clsProductAddOnSelectionDTO(int addOnID, string imageUrl, bool isSelected)
        {
            AddOnID = addOnID;
            ImageURL = imageUrl;
            IsSelected = isSelected;
        }
    }
}
