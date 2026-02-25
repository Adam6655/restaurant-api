using Microsoft.Data.SqlClient;
using RestaurantData;
using RestaurantDTOs;
using System;
using System.Data;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace RestaurantData
{
    public class clsCartsData
    {
        public static List<clsCartDTO> GetCart(int OrderID)
        {
            var CartItemsList = new List<clsCartDTO>();

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_GetCart", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@OrderID", OrderID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int ProductIDIndex = reader.GetOrdinal("ProductID");
                    int ProductNameIndex = reader.GetOrdinal("ProductName");
                    int ProductDescriptionIndex = reader.GetOrdinal("ProductDescription");
                    int ProductPriceIndex = reader.GetOrdinal("Price");
                    int CategoryIDIndex = reader.GetOrdinal("CategoryID");
                    int ProductImageUrlIndex = reader.GetOrdinal("ProductImageUrl");
                    int CaloriesIndex = reader.GetOrdinal("Calories");
                    int ProductIsActiveIndex = reader.GetOrdinal("IsActive");
                    int QuantityIndex = reader.GetOrdinal("Quantity");
                    int NotesIndex = reader.GetOrdinal("Notes");
                    int AddOnIDIndex = reader.GetOrdinal("AddOnID");
                    int AddOnNameIndex = reader.GetOrdinal("AddOnName");
                    int AddOnPriceIndex = reader.GetOrdinal("AddOnPrice");
                    int AddOnImageUrlIndex = reader.GetOrdinal("AddOnImageUrl");
                    int AddOnIsActiveIndex = reader.GetOrdinal("AddOnIsActive");

                    if (reader.Read())
                    {
                        string? Notes = !reader.IsDBNull(NotesIndex) ? reader.GetString(NotesIndex) : null;
                        int? AddOnID = !reader.IsDBNull(AddOnIDIndex) ? reader.GetInt32(AddOnIDIndex) : null;

                        CartItemsList.Add(new clsCartDTO(new clsProductDTO(reader.GetInt32(ProductIDIndex),
                                reader.GetString(ProductNameIndex), reader.GetString(ProductDescriptionIndex),
                                reader.GetDecimal(ProductPriceIndex), reader.GetInt32(CategoryIDIndex),
                                reader.GetString(ProductImageUrlIndex), reader.GetInt32(CaloriesIndex),
                                reader.GetBoolean(ProductIsActiveIndex)), reader.GetByte(QuantityIndex), Notes,
                                AddOnID == null ? [] : new List<clsAddOnDTO> { new clsAddOnDTO(reader.GetInt32(AddOnIDIndex),
                                reader.GetString(AddOnNameIndex),reader.GetInt32(AddOnPriceIndex),
                                reader.GetString(AddOnImageUrlIndex),reader.GetBoolean(AddOnIsActiveIndex)) }
                        ));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return CartItemsList;
        }
        public static decimal CalculateCartTotalAmount(List<clsCartDTO> Cart)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_CalculateCartTotalAmount2", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter tvpParam = command.Parameters.AddWithValue("@Cart", ConvertCartToDataTable(Cart));
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "CartTableType";

                var TotalAmount = new SqlParameter("@CartTotalAmount", SqlDbType.TinyInt)
                {
                    Direction = ParameterDirection.Output
                };

                command.Parameters.Add(TotalAmount);

                connection.Open();
                command.ExecuteNonQuery();

                return (decimal)TotalAmount.Value;
            }
        }
        public static List<clsCartDTO> SyncCartItemsWithDatabase(List<clsCartDTO> Cart)
        {
            List<clsCartDTO> CartList = new List<clsCartDTO>();

            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_syncCartItemsWithDatabase2", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter tvpParam = command.Parameters.AddWithValue("@Cart", ConvertCartToDataTable(Cart));
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "CartTableType";

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    int? PreviousProductID = null;
                    int? CurrentProductID = null;
                    int? Counter = null;
                    clsCartDTO? CartDTO = null;

                    int ProductIDIndex = reader.GetOrdinal("ProductID");
                    int ProductNameIndex = reader.GetOrdinal("ProductName");
                    int ProductDescriptionIndex = reader.GetOrdinal("ProductDescription");
                    int ProductPriceIndex = reader.GetOrdinal("Price");
                    int CategoryIDIndex = reader.GetOrdinal("CategoryID");
                    int ProductImageUrlIndex = reader.GetOrdinal("ProductImageUrl");
                    int CaloriesIndex = reader.GetOrdinal("Calories");
                    int ProductIsActiveIndex = reader.GetOrdinal("IsActive");
                    int QuantityIndex = reader.GetOrdinal("Quantity");
                    int NotesIndex = reader.GetOrdinal("Notes");
                    int AddOnIDIndex = reader.GetOrdinal("AddOnID");
                    int AddOnNameIndex = reader.GetOrdinal("AddOnName");
                    int AddOnPriceIndex = reader.GetOrdinal("AddOnPrice");
                    int AddOnImageUrlIndex = reader.GetOrdinal("AddOnImageUrl");
                    int AddOnIsActiveIndex = reader.GetOrdinal("AddOnIsActive");

                    if (reader.Read())
                    {
                        CurrentProductID = reader.GetInt32(ProductIDIndex);

                        if (PreviousProductID != CurrentProductID)
                        {
                            if (PreviousProductID != null)
                            {
                                CartList.Add(CartDTO!);
                            }

                            string? Notes = !reader.IsDBNull(NotesIndex) ? reader.GetString(NotesIndex) : null;
                            int? AddOnID = !reader.IsDBNull(AddOnIDIndex) ? reader.GetInt32(AddOnIDIndex) : null;

                            CartDTO = new clsCartDTO(new clsProductDTO(reader.GetInt32(ProductIDIndex),
                                reader.GetString(ProductNameIndex), reader.GetString(ProductDescriptionIndex),
                                reader.GetDecimal(ProductPriceIndex), reader.GetInt32(CategoryIDIndex),
                                reader.GetString(ProductImageUrlIndex), reader.GetInt32(CaloriesIndex),
                                reader.GetBoolean(ProductIsActiveIndex)), reader.GetByte(QuantityIndex), Notes,
                                AddOnID == null ? [] : new List<clsAddOnDTO> { new clsAddOnDTO(reader.GetInt32(AddOnIDIndex),
                                reader.GetString(AddOnNameIndex),reader.GetInt32(AddOnPriceIndex),
                                reader.GetString(AddOnImageUrlIndex),reader.GetBoolean(AddOnIsActiveIndex)) });

                            Counter = 1;
                        }

                        if (Counter > 1)
                        {
                            CartDTO!.AddOns.Add(new clsAddOnDTO(reader.GetInt32(AddOnIDIndex),
                                reader.GetString(AddOnNameIndex),
                                reader.GetInt32(AddOnPriceIndex),
                                reader.GetString(AddOnImageUrlIndex),
                                reader.GetBoolean(AddOnIsActiveIndex)));
                        }

                        Counter++;
                        PreviousProductID = CurrentProductID;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return CartList;
        }
        public static DataTable ConvertCartToDataTable(List<clsCartDTO> Cart)
        {
            DataTable CartTable = new DataTable();
            CartTable.Columns.Add("ProductID", typeof(int));
            CartTable.Columns.Add("ProductName", typeof(string));
            CartTable.Columns.Add("ProductDescription", typeof(string));
            CartTable.Columns.Add("ProductPrice", typeof(decimal));
            CartTable.Columns.Add("CategoryID", typeof(int));
            CartTable.Columns.Add("ProductImageUrl", typeof(string));
            CartTable.Columns.Add("Calories", typeof(int));
            CartTable.Columns.Add("ProductIsActive", typeof(bool));
            CartTable.Columns.Add("Quantity", typeof(byte));
            CartTable.Columns.Add("Notes", typeof(string));
            CartTable.Columns.Add("AddOnID", typeof(int));
            CartTable.Columns.Add("AddOnName", typeof(string));
            CartTable.Columns.Add("AddOnPrice", typeof(decimal));
            CartTable.Columns.Add("AddOnImageURL", typeof(string));
            CartTable.Columns.Add("AddOnIsActive", typeof(bool));

            for (int i = 0; i < Cart.Count; i++)
            {
                if (Cart[i].AddOns != null)
                {
                    for (int c = 0; c < Cart[i].AddOns.Count; c++)
                    {
                        CartTable.Rows.Add(Cart[i].Product.ProductID, Cart[i].Product.ProductName,
                        Cart[i].Product.ProductDescription, Cart[i].Product.Price, Cart[i].Product.CategoryID,
                        Cart[i].Product.ImageUrl, Cart[i].Product.Calories, Cart[i].Product.IsActive,
                        (c == 0) ? Cart[i].Quantity : DBNull.Value,
                        (c == 0) ? Cart[i].Notes ?? (object)DBNull.Value : DBNull.Value,
                        Cart[i].AddOns[c].AddOnID, Cart[i].AddOns[c].AddOnName, Cart[i].AddOns[c].Price,
                        Cart[i].AddOns[c].ImageURL, Cart[i].AddOns[c].IsActive);
                    }
                }
                else
                {
                    CartTable.Rows.Add(Cart[i].Product.ProductID, Cart[i].Product.ProductName,
                        Cart[i].Product.ProductDescription, Cart[i].Product.Price, Cart[i].Product.CategoryID,
                        Cart[i].Product.ImageUrl, Cart[i].Product.Calories, Cart[i].Product.IsActive,
                        Cart[i].Quantity, Cart[i].Notes ?? (object)DBNull.Value, DBNull.Value, DBNull.Value,
                        DBNull.Value, DBNull.Value, DBNull.Value);
                }
            }
            return CartTable;
        }
        public static bool IsCartUpToDate(List<clsCartDTO> CheckOutCart)
        {
            using (var connection = new SqlConnection(clsDataSettings.ConnectionString))
            using (var command = new SqlCommand("SP_IsCartUpToDate", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter tvpParam = command.Parameters.AddWithValue("@Cart", 
                    ConvertCartToDataTable(CheckOutCart));
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "CartTableType";

                var ReturnParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                ReturnParam.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                return (bool)ReturnParam.Value;
            }
        }
    }
}
//GetCart,CalculateCartTotalAmount2,SyncCartItemsWithDatabase2
//Errors in this page (CalculateCartTotalAmount,SyncCartItemsWithDatabase) because of commenting clsCheckoutClass
// ,,    ,,   ,,  ,,  (GetCartItemsByOrderID) because of commenting clsCart

//if (Cart[i]?.Product != null && Cart[i].AddOns != null && Cart[i].AddOns.Count > 0)
//{
//    // First add-on
//    var addOn = Cart[i].AddOns[0];

//    CartTable.Rows.Add(
//        Cart[i].Product.ProductID,
//        Cart[i].Product.ProductName,
//        Cart[i].Product.ProductDescription,
//        Cart[i].Product.Price,
//        Cart[i].Product.CategoryID,
//        Cart[i].Product.ImageUrl,
//        Cart[i].Product.Calories,
//        Cart[i].Product.IsActive,
//        Cart[i].Quantity,
//        Cart[i].Notes ?? (object)DBNull.Value,
//        addOn.AddOnID,
//        addOn.AddOnName,
//        addOn.Price,
//        addOn.ImageURL,
//        addOn.IsActive
//    );

//    // Remaining add-ons
//    for (int c = 1; c < Cart[i].AddOns.Count; c++)
//    {
//        var a = Cart[i].AddOns[c];

//        CartTable.Rows.Add(
//            Cart[i].Product.ProductID,
//            Cart[i].Product.ProductName,
//            Cart[i].Product.ProductDescription,
//            Cart[i].Product.Price,
//            Cart[i].Product.CategoryID,
//            Cart[i].Product.ImageUrl,
//            Cart[i].Product.Calories,
//            Cart[i].Product.IsActive,
//            DBNull.Value,
//            DBNull.Value,
//            a.AddOnID,
//            a.AddOnName,
//            a.Price,
//            a.ImageURL,
//            a.IsActive
//        );
//    }
//}
