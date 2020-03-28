using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetStore.Models;

namespace PetStore.Interfaces
{
   public interface IPetService
    {
        IEnumerable<Pet> GetAllPets();
        Pet GetPetById(int id);
        IEnumerable<Pet> GetPetsByBreed(string breed);
        bool OldEnoughToAdopt(DateTime DOB);

    }
}
