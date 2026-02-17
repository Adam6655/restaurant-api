using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDTOs
{
    public class clsCoinTransactionDTO
    {
        public byte TransactionStatus { get; set; }
        public DateOnly TransactionDate { get; set; }
        public byte Coins { get; set; }
        public clsCoinTransactionDTO(byte Transactionstatus, DateOnly Transactiondate, byte coins)
        {
            TransactionStatus = Transactionstatus;
            TransactionDate = Transactiondate;
            Coins = coins;
        }
    }
    public class clsCoinSavingsSummaryDTO
    {
        List<clsCoinTransactionDTO> CoinTransactionDTO { get; set; }
        public int TotalUserCoins { get; set; }
        public decimal TotalCoinValue {  get; set; }
        public decimal TotalSavings { get ; set; }
        public clsCoinSavingsSummaryDTO(List<clsCoinTransactionDTO> coinTransactionDTO, int totalUserCoins, decimal totalCoinValue, decimal totalSavings)
        {
            CoinTransactionDTO = coinTransactionDTO;
            TotalUserCoins = totalUserCoins;
            TotalCoinValue = totalCoinValue;
            TotalSavings = totalSavings;
        }
    }
}
