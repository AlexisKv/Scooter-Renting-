using System;
using ScooterRentals.Interfaces;

namespace ScooterRentals
{
    public class NewYearRates : IRentalCalculator
    {
        //Special Scooter rates for NewYear
        public decimal CalculateScooterFee(RentTimer rentTimer)
        {
            var endTimeFinal = rentTimer.endTime ?? DateTime.Now;
            var usageTime = endTimeFinal - rentTimer.startTime;
            return  3 * Convert.ToDecimal(usageTime.TotalMinutes )* rentTimer._price ;
        }
    }
}