using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetStore.Services;

namespace PetStore.Tests
{
    [TestClass]
    public class PetServiceTests
    {
        [TestMethod]
        public void PetService_OldEnough_Positive()
        {
            PetService petService = new PetService();
            DateTime dateOfBirth = new DateTime(2000, 1, 1);
            var results = petService.OldEnoughToAdopt(dateOfBirth);
            Assert.AreEqual(true, results);
        }

        [TestMethod]
        public void PetService_OldEnough_Negative()
        {
            PetService petService = new PetService();
            DateTime dateOfBirth = DateTime.Now;//This will return the machines current date
            var results = petService.OldEnoughToAdopt(dateOfBirth);

            Assert.AreEqual(false, results);
        }
    }
}
