using System;

namespace ScooterRentals.Exceptions
{
    public class InvalidIdException : Exception
    {
        public InvalidIdException(): base($"ID cannot be null or empty."){ }
    }
}