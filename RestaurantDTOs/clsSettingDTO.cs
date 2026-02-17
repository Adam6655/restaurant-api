using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDTOs
{
    public class clsSettingDTO
    {
        public byte CoinsToEarn { get; set; }
        public int AmountToSpend { get; set; }
        public decimal DeliveryFeePerKilo { get; set; }
        public int CoinWorth { get; set; }
        public string RestaurantAddress { get; set; }
        public decimal RestaurantLatitude { get; set; }
        public decimal RestaurantLongitude { get; set; }
        public string Currency { get; set; }
        public clsSettingDTO(byte coinsToEarn, int amountToSpend, decimal deliveryFeePerKilo, int coinWorth, string restaurantAddress, decimal restaurantLatitude, decimal restaurantLongitude, string currency)
        {
            CoinsToEarn = coinsToEarn;
            AmountToSpend = amountToSpend;
            DeliveryFeePerKilo = deliveryFeePerKilo;
            CoinWorth = coinWorth;
            RestaurantAddress = restaurantAddress;
            RestaurantLatitude = restaurantLatitude;
            RestaurantLongitude = restaurantLongitude;
            Currency = currency;
        }
    }
}
