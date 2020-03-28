using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetStore.Interfaces;
using PetStore.Models;


namespace PetStore.Services
{
    public class PetService:IPetService
    {
        private ApplicationDbContext context;

        public PetService()
        {
            this.context = new ApplicationDbContext();
        }

        public PetService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Pet> GetAllPets()
        {
            return context.Pets.ToList();
        }

        public Pet GetPetById(int id)
        {
            return context.Pets.Find(id);
        }

        public IEnumerable<Pet> GetPetsByBreed(string breed)
        {
            return context.Pets.Where(p => p.Breed.Equals(breed));
        }

        public virtual bool OldEnoughToAdopt(DateTime DOB)
        {
            bool isOldEnoughToAdopt = false;
            var age = DateTime.Now.Year - DOB.Year;
            if (DOB > DateTime.Today.AddYears(-age))
                age = age - 1;


            if (age >= 18)
                isOldEnoughToAdopt = true;

            return isOldEnoughToAdopt;
        }
    }
}