using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetStore.Models.ViewModel
{
    public class EditPetViewModel
    {

        public int ID { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public bool isMale { get; set; }

        public string Breed { get; set; }

        public IEnumerable<SelectListItem> DropDownBreed { get; set; }
    }
}