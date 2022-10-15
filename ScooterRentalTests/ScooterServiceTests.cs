using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScooterRentals;
using ScooterRentals.Exceptions;
using ScooterRentals.Interfaces;

namespace ScooterRentalTests
{
    [TestClass]
    public class ScooterServiceTests
    {
        private IScooterService _scooterService;
        private List<Scooter> _inventory;

        [TestInitialize]
        public void Setup()
        {
            _inventory = new List<Scooter>();
            _scooterService = new ScooterService(_inventory);
        }

        [TestMethod]
        public void AddScooter_AddValidScooter_ScooterAdded()
        {
            _scooterService.AddScooter("1", 0.2m);
            _inventory.Count.Should().Be(1);

        }

        [TestMethod]
        public void AddScooter_AddScooterTwice_ThrowsDuplicateScooterException()
        {
            _scooterService.AddScooter("1", 0.2m);

            Action act = () => _scooterService.AddScooter("1", 0.2m);
            act.Should().Throw<DuplicateScooterException>().WithMessage("Scooter 1 Already exist");
        }

        [TestMethod]
        public void AddScooter_AddScooterWithPriceZeroOrLess_ThrowsInvalidPriceException()
        {
            Action act = () => _scooterService.AddScooter("1", -0.2m);
            act.Should().Throw<InvalidPriceException>().WithMessage("Given price -0.2 not valid");
        }
        
        [TestMethod]
        public void AddScooter_AddScooterNullOrEmptyID_ThrowsInvalidPriceException()
        {
            Action act = () => _scooterService.AddScooter("", 0.2m);
            act.Should().Throw<InvalidIdException>().WithMessage("ID cannot be null or empty.");
        }

        [TestMethod]
        public void RemoveScooter_ScooterExists_ScooterRemove()
        {
            _inventory.Add(new Scooter("1", 0.2m));
            _scooterService.RemoveScooter("1");
            _inventory.Any(scooter => scooter.Id == "1").Should().BeFalse();
            _inventory.Count.Should().Be(0);

        }
        
        [TestMethod]
        public void RemoveScooter_ScooterDoesntExist_ThrowScooterDoesntExistException()
        {
          Action act = () => _scooterService.RemoveScooter("1");
          act.Should().Throw<ScooterDoesntExist>().WithMessage("Scooter with id 1 doesn't exist.");
        }
        
        [TestMethod]
        public void RemoveScooter_RemoveScooterNullOrEmptyIDGiven_ThrowsInvalidIDException()
        {
            Action act = () => _scooterService.RemoveScooter("");
            act.Should().Throw<InvalidIdException>().WithMessage("ID cannot be null or empty.");
        }

        [TestMethod]
        public void GetScooters_GetScootersShowingAllScooters()
        {
            _scooterService.AddScooter("1", 0.2m);
            _scooterService.AddScooter("2", 0.25m);
            var scooterList = _scooterService.GetScooters();

            scooterList.Should().ContainEquivalentOf(new Scooter("1", 0.2m));
            scooterList.Should().ContainEquivalentOf(new Scooter("2", 0.25m));
            scooterList.Count.Should().Be(2);
        }

        [TestMethod]
        public void GetScooters_ScootersDoesntExist_ThrowInventoryIsEmptyExceptions()
        {
            var scooterList = _scooterService;
            Action act = () => _scooterService.GetScooters();
            act.Should().Throw<InventoryIsEmptyExceptions>().WithMessage("Your inventory is empty.");
        }

        [TestMethod]
        public void GetScooterById_GetScooterByIdIsShowingYourScooterWithYourId()
        {
            _scooterService.AddScooter("1", 0.2m);
            _scooterService.AddScooter("2", 0.25m);

            var expectedResult = new Scooter("2", 0.25m);
            _scooterService.GetScooterById("2").Should().BeEquivalentTo(expectedResult);

        }
        
        [TestMethod]
        public void GetScooterById_ScooterWithYourIdDoesntExist_ThrowInventoryDoesntHaveYourScooterExceptions()
        {
            Action act = () => _scooterService.GetScooterById("2");
            act.Should().Throw<ScooterDoesntExist>().WithMessage($"Scooter with id 2 doesn't exist.");
        }
        
    }
}