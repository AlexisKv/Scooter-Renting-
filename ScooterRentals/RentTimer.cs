using System;

namespace ScooterRentals
{
    public class RentTimer
    {
        public DateTime startTime;
        public DateTime? endTime;
        public string id;
        public decimal _price;

        public RentTimer(DateTime startTime, string id, decimal price)
        {
            this.startTime = startTime;
            this.id = id;
            _price = price;
            endTime = null;
        }

    }
}