using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScooterRentals.Interfaces;

namespace ScooterRentals
{
    public class RentalCalculator : IRentalCalculator
    {
        public decimal CalculateScooterFee(RentTimer rentTimer)
        {
            Decimal finalRentalPrice = 0;
            
            var endTimeFinal = rentTimer.endTime ?? DateTime.Now;
            var usageTime = endTimeFinal - rentTimer.startTime;

            if (rentTimer.startTime.Day == endTimeFinal.Day )
            {
                finalRentalPrice = Convert.ToDecimal(usageTime.TotalMinutes) * rentTimer._price;
                
                if (finalRentalPrice >= 20)
                {
                    finalRentalPrice = 20;
                }
            }
            
            else
            {
                var firstDayIncome = (decimal)(rentTimer.startTime.Date.AddDays(1) - rentTimer.startTime).TotalMinutes * rentTimer._price;
                var lastDayIncome = (decimal)(endTimeFinal - endTimeFinal.Date).TotalMinutes * rentTimer._price;
                
                if (firstDayIncome >= 20)
                {
                    firstDayIncome = 20;
                }
                if (lastDayIncome >= 20)
                {
                    lastDayIncome = 20;
                }

                var totalDaysOfRenting = (endTimeFinal - rentTimer.startTime).Days - 1;
                finalRentalPrice = totalDaysOfRenting * 20 + lastDayIncome + firstDayIncome;
            }

            return finalRentalPrice;
        }

    }
}