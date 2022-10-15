using System;

namespace ScooterRentals.Exceptions
{
    public class DuplicateScooterException : Exception
    {
        public DuplicateScooterException(string id) : base($"Scooter {id} Already exist") { }
    }
}