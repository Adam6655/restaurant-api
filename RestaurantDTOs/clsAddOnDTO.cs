namespace RestaurantDTOs
{
    public class clsAddOnDTO
    {
        public clsAddOnDTO(int AddOnid, string addOnName, decimal price, string imageURL, bool active)
        {
            this.AddOnID = AddOnid;
            this.AddOnName = addOnName;
            this.Price = price;
            this.ImageURL = imageURL;
            this.IsActive = active;
        }


        public int AddOnID { get; set; }
        public string AddOnName { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }
        public bool IsActive { get; set; }
    }
    public class clsAddOnProductSelectionDTO
    {
        public int ProductID { get; set; }
        public string ImageURL {  get; set; }
        public bool IsSelected { get; set; }
        public clsAddOnProductSelectionDTO(int productID, string imageURL, bool isSelected)
        {
            ProductID = productID;
            ImageURL = imageURL;
            IsSelected = isSelected;
        }
    }
}
