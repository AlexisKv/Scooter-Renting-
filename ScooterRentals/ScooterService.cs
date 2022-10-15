using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using ScooterRentals.Exceptions;
using ScooterRentals.Interfaces;

namespace ScooterRentals
{
    public class ScooterService : IScooterService
    {
        
        public List<Scooter> _scooters;
        
        public ScooterService(List<Scooter> inventory)
        {
            _scooters = inventory;
        }
        
        public void AddScooter(string id, decimal pricePerMinute)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidIdException();
            }
            
            if (pricePerMinute <= 0)
            {
                throw new InvalidPriceException(pricePerMinute);
            }
            
            if (_scooters.Any(scooter => scooter.Id == id))
            {
                throw new DuplicateScooterException(id);
            }
            _scooters.Add(new Scooter(id, pricePerMinute));
        }

        public void RemoveScooter(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidIdException();
            }
            
            var scooter = _scooters.FirstOrDefault(scooter => scooter.Id == id);
            
            if (scooter == null)
            {
                throw new ScooterDoesntExist(id);
            }
            _scooters.Remove(scooter);
        }

        public IList<Scooter> GetScooters()
        {
            if (_scooters.Count == 0)
            {
                throw new InventoryIsEmptyExceptions();
            }
            
            return _scooters.ToArray();
        }

        public Scooter GetScooterById(string scooterId)
        {
            if (_scooters.FirstOrDefault(scooter => scooter.Id == scooterId) == null)
            {
                throw new ScooterDoesntExist(scooterId);
            }
            return _scooters.FirstOrDefault(scooter => scooter.Id == scooterId);
        }
    }
}