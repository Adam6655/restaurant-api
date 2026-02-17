using System;
using System.Data;
using RestaurantData;
using RestaurantDTOs;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace RestaurantBusiness
{
    public class clsCoinTransaction
    {// I guess we wont need this class since will not include any method and wont add or update any transaction
        public byte Status {  get; set; }
        public DateOnly Date {  get; set; }
        public byte Coins { get; set; }

    }
}
//public class clsCoinTransactionDTO
//{
//    public byte Status { get; set; }
//    public DateOnly Date { get; set; }
//    public byte Coins { get; set; }
//    public clsCoinTransactionDTO(byte status, DateOnly date, byte coins)
//    {
//        Status = status;
//        Date = date;
//        Coins = coins;
//    }
//}