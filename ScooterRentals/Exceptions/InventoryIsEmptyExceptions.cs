using System;

namespace ScooterRentals.Exceptions
{
    public class InventoryIsEmptyExceptions : Exception
    {
        public InventoryIsEmptyExceptions() : base($"Your inventory is empty.") { }
    }
}