namespace ScooterRentals.Interfaces
{
    public interface IRentalCalculator
    {
        public decimal CalculateScooterFee(RentTimer rentTimer);
    }
}