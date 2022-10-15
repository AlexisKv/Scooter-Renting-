using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ScooterRentals;
using ScooterRentals.Exceptions;
using ScooterRentals.Interfaces;

namespace ScooterRentalTests
{
    [TestClass]
    public class RentalCompanyTests
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
        public void InitializeRentalCompany_CheckIsNameGiven()
        {
            _rentalCompany.Name.Should().Be("Dell");
        }

        [TestMethod]
        public void InitializeRentalCompany_NameIsNullOrEmptyID_ThrowsInvalidNameException()
        {
            Action act = () => _rentalCompany = new RentalCompany("", _scooterService, _rentalTimer, _rentalCalculator);
            act.Should().Throw<InvalidNameException>().WithMessage("Name cannot be null or empty.");
        }

        [TestMethod]
        public void StartRent_ScooterHasRentedStatus()
        {
            _rentalCompany.StartRent("1");
            _scooterService.GetScooterById("1").IsRented.Should().BeTrue();
            _rentalTimer.Count.Should().Be(1);

        }

        [TestMethod]
        public void EndRent_ScooterHasEndedRetingStatus()
        {
            _rentalCompany.StartRent("1");
            _rentalCompany.EndRent("1");
            _scooterService.GetScooterById("1").IsRented.Should().BeFalse();

        }

        [TestMethod]
        public void EndRent_ScooterRentingPricePer1hShowingCorrectPrice()
        {
            
            _rentalCompany.StartRent("1");
            _rentalTimer[0].startTime = DateTime.Now.AddMinutes(-60);
            _rentalCompany.EndRent("1");
            Convert.ToInt32(_rentalCalculator.CalculateScooterFee(_rentalTimer[0])).Should().Be(12);
        }

        [TestMethod]
        public void EndRent_ScooterRentingMaxPricePerDayIs20currency()
        {
            _rentalCompany.StartRent("1");
            _rentalTimer[0].startTime = new DateTime(2015, 10, 01, 5, 00, 00);
            _rentalCompany.EndRent("1");
            _rentalTimer[0].endTime = new DateTime(2015, 10, 01, 20, 00, 00);
            Convert.ToInt32(_rentalCalculator.CalculateScooterFee(_rentalTimer[0])).Should().Be(20);
        }
        
        [TestMethod]
        public void EndRent_ScooterRentingMaxPricePer5daysIsCalculatedWithMaxPricePerDay()
        {
            _rentalCompany.StartRent("1");
            _rentalTimer[0].startTime = new DateTime(2015, 10, 01, 00, 00, 00);
            _rentalCompany.EndRent("1");
            _rentalTimer[0].endTime = new DateTime(2015, 10, 06, 1, 00, 00);
            _rentalCalculator.CalculateScooterFee(_rentalTimer[0]).Should().Be(112);
        }

        [TestMethod]
        public void CalculateIncome_OneScooterProfitFrom1monthCalculation()
        {
            _rentalCompany.StartRent("1");
            _rentalTimer[0].startTime = new DateTime(2015, 10, 01, 00, 00, 00);
            _rentalCompany.EndRent("1");
            _rentalTimer[0].endTime = new DateTime(2015, 10, 06, 1, 00, 00);

            _rentalCompany.StartRent("1");
            _rentalTimer[1].startTime = new DateTime(2015, 10, 07, 01, 00, 00);
            _rentalCompany.EndRent("1");
            _rentalTimer[1].endTime = new DateTime(2015, 10, 07, 02, 00, 00);

            _rentalCompany.CalculateIncome(2015, false).Should().Be(124);
        }
        
        [DataRow(2015, true, 52)]
        [DataRow(2015, false, 52)]
        [DataRow(null, true, 196)]
        [DataRow(null, false, 0)]

        [DataTestMethod]
        public void CalculateIncome_GlobalTest(int? year, bool includeNotCompletedRentals, int result)
        {
            _rentalTimer.Add(new RentTimer( new DateTime(2015, 10, 07, 01, 00, 00), "1", 0.2m));
            _rentalTimer.Add(new RentTimer( new DateTime(2015, 10, 08, 01, 00, 00), "2", 0.2m));
            _rentalTimer.Add(new RentTimer( new DateTime(2016, 10, 01, 01, 00, 00), "3", 0.2m));
            _rentalTimer.Add(new RentTimer( new DateTime(2016, 12, 31, 02, 00, 00), "4", 0.2m));
            
            _rentalTimer[0].endTime = new DateTime(2015, 10, 07, 23, 00, 00);//20
            _rentalTimer[1].endTime = new DateTime(2015, 10, 09, 01, 00, 00);//32
            _rentalTimer[2].endTime = new DateTime(2016, 10, 07, 01, 00, 00);//132
            _rentalTimer[3].endTime = new DateTime(2017, 01, 01, 01, 00, 00);//32
            
            _rentalCompany.CalculateIncome(year, includeNotCompletedRentals).Should().Be(result);
        }
    }
}