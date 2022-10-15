// using System;
// using System.Collections.Generic;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using ScooterRentals;
// using ScooterRentals.Interfaces;
//
// namespace ScooterRentalTests
// {
//     public class ScooterRentalTests
//     {
//         [TestClass]
//         public class RentalCompanyTests
//         {
//             private IScooterService _scooterService;
//             private List<Scooter> _inventory;
//             private IRentalCompany _rentalCompany;
//             private List<RentTimer> _rentalTimer;
//
//             [TestInitialize]
//             public void Setup()
//             {
//                 _inventory = new List<Scooter>();
//                 _scooterService = new ScooterService(_inventory);
//                 _rentalTimer = new List<RentTimer>();
//                 _rentalCompany = new RentalCompany("Dell", _scooterService,_rentalTimer);
//                 _scooterService.AddScooter("1", 0.2m);
//
//             }
//     }
// }