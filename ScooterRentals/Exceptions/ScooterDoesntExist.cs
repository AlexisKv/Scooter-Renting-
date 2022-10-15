using System;

namespace ScooterRentals.Exceptions
{
    public class ScooterDoesntExist : Exception
    {
        public ScooterDoesntExist(string id) : base($"Scooter with id {id} doesn't exist.") { }
    }
}