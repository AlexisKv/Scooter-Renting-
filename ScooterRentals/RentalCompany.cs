using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using ScooterRentals.Exceptions;
using ScooterRentals.Interfaces;

namespace ScooterRentals
{
    public class RentalCompany : IRentalCompany
    {
        private IScooterService _scooterService;
        public string Name { get; }
        public List<RentTimer> _rentTimer;
        private IRentalCalculator _rentalCalculator;
        
        public RentalCompany(string companyName, IScooterService scooterService
            , List<RentTimer> rentTimer, IRentalCalculator rentalCalculator)
        {
            _rentalCalculator = rentalCalculator;
            _scooterService = scooterService;
            _rentTimer = rentTimer;
            if (string.IsNullOrEmpty(companyName))
            {
                throw new InvalidNameException();
            }
            
            Name = companyName;
        }
        
        public void StartRent(string id)
        {
            var scooter = _scooterService.GetScooterById(id);
            _rentTimer.Add(new RentTimer(DateTime.Now, id, scooter.PricePerMinute));
            _scooterService.GetScooterById(id).IsRented = true;
        }

        public decimal EndRent(string id)
        {
            var scooter = _scooterService.GetScooterById(id);
            var scooterWithEnderTimer = _rentTimer.LastOrDefault(s => s.id == scooter.Id && s.endTime == null);
            scooterWithEnderTimer.endTime = DateTime.Now;
            scooter.IsRented = false;
            return _rentalCalculator.CalculateScooterFee(scooterWithEnderTimer);
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            decimal TotalIncome = 0;

            if (includeNotCompletedRentals && year == null)
            {
                foreach (var scooter in _rentTimer)
                {
                    TotalIncome += _rentalCalculator.CalculateScooterFee(scooter);
                }
            }

            if (includeNotCompletedRentals == false && year == null)
            {
                foreach (var scooter in _rentTimer.Where(x => x.endTime == null))
                {
                    TotalIncome += _rentalCalculator.CalculateScooterFee(scooter);
                }
            }

            if (includeNotCompletedRentals && year != null)
            {
                foreach (var scooter in _rentTimer.Where(x =>x.endTime == null|| x.endTime.Value.Year == year))
                {
                    TotalIncome += _rentalCalculator.CalculateScooterFee(scooter);
                }
            }
            
            if (includeNotCompletedRentals == false && year != null)
            {
                foreach (var scooter in _rentTimer.Where(x => x.endTime.HasValue && x.endTime.Value.Year == year ))
                {
                    TotalIncome += _rentalCalculator.CalculateScooterFee(scooter);
                }
            }
            return TotalIncome;

        }
    }
}