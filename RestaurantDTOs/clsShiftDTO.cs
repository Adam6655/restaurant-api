using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDTOs
{
    public class clsShiftDTO
    {
        public int ShiftID { get; set; }
        public byte ShiftDay { get; set; }
        public TimeSpan ShiftStart { get; set; }
        public TimeSpan ShiftEnd { get; set; }
        public clsShiftDTO(int shiftID, byte shiftDay, TimeSpan shiftStart, TimeSpan shiftEnd)
        {
            ShiftID = shiftID;
            ShiftDay = shiftDay;
            ShiftStart = shiftStart;
            ShiftEnd = shiftEnd;
        }
    }
}
