using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScooterRentals;
using ScooterRentals.Interfaces;

namespace ScooterRentalTests
{
    [TestClass]
    public class RentalCalculatorTests
    {
        private IScooterService _scooterService;
        private List<Scooter> _inventory;
        private IRentalCompany _rentalCompany;
        private List<RentTimer> _rentalTimer;
        private IRentalCalculator _rentalCalculator;

        [TestInitialize]
        public void Setup()
        {
            _inventory = new List<Scooter>();
            _scooterService = new ScooterService(_inventory);
            _rentalTimer = new List<RentTimer>();
            _rentalCalculator = new RentalCalculator();
            _rentalCompany = new RentalCompany("Dell", _scooterService,_rentalTimer, _rentalCalculator);
            _scooterService.AddScooter("1", 0.2m);
        }
        
        [TestMethod]
        public void CalculateScooterFee_ShouldReturnPricePer1h()
        {
            _rentalCompany.StartRent("1");
            _rentalTimer[0].startTime = new DateTime(2015, 10, 01, 00, 00, 00);
            _rentalCompany.EndRent("1");
            _rentalTimer[0].endTime = new DateTime(2015, 10, 01, 1, 00, 00);
            _rentalCalculator.CalculateScooterFee(_rentalTimer[0]).Should().Be(12);
        }
        
        [TestMethod]
        public void CalculateScooterFee_ShouldReturnMaxPricePer1Day()
        {
            _rentalCompany.StartRent("1");
            _rentalTimer[0].startTime = new DateTime(2015, 10, 01, 00, 00, 00);
            _rentalCompany.EndRent("1");
            _rentalTimer[0].endTime = new DateTime(2015, 10, 01, 23, 00, 00);
            _rentalCalculator.CalculateScooterFee(_rentalTimer[0]).Should().Be(20);
        }
        
        [TestMethod]
        public void CalculateScooterFee_ShouldReturnMaxPricePer1DayAndRemainderOfNextDay()
        {
            _rentalCompany.StartRent("1");
            _rentalTimer[0].startTime = new DateTime(2015, 10, 01, 00, 00, 00);
            _rentalCompany.EndRent("1");
            _rentalTimer[0].endTime = new DateTime(2015, 10, 02, 1, 00, 00);
            _rentalCalculator.CalculateScooterFee(_rentalTimer[0]).Should().Be(32);
        }
        
        [TestMethod]
        public void CalculateScooterFee_ShouldReturnRemainderOfFirstDayAndMaxPricePerNextDay()
        {
            _rentalCompany.StartRent("1");
            _rentalTimer[0].startTime = new DateTime(2015, 10, 01, 23, 30, 00);
            _rentalCompany.EndRent("1");
            _rentalTimer[0].endTime = new DateTime(2015, 10, 02, 23, 59, 00);
            _rentalCalculator.CalculateScooterFee(_rentalTimer[0]).Should().Be(26);
        }
        
        [TestMethod]
        public void CalculateScooterFee_ShouldReturnMaxPricePer5FullDays()
        {
            _rentalCompany.StartRent("1");
            _rentalTimer[0].startTime = new DateTime(2015, 10, 01, 01, 00, 00);
            _rentalCompany.EndRent("1");
            _rentalTimer[0].endTime = new DateTime(2015, 10, 05, 23, 59, 00);
            _rentalCalculator.CalculateScooterFee(_rentalTimer[0]).Should().Be(100);
        }
    }
}