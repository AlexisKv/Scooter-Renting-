using System;

namespace ScooterRentals.Exceptions
{
    public class InvalidNameException : Exception
    {
        public InvalidNameException(): base($"Name cannot be null or empty."){ }
    }
}