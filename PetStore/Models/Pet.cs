using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PetStore.Attributes;

namespace PetStore.Models
{
    public class Pet
    {
        public int ID { get; set; }

        [Required]
        [NoDigits]
        public string Name { get; set; }

        [NonNegative]
        public int Age { get; set; }

        [Required]
        public bool isMale { get; set; }

        public string Breed { get; set; }

        public ApplicationUser Owner { get; set; }

    }
}