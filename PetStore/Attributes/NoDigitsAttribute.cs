using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PetStore.Attributes
{
    public class NoDigitsAttribute : ValidationAttribute
    {
        public NoDigitsAttribute() : base("No numbers allowed!")
        {

        }

        public override bool IsValid(object value)
        {

            bool doesContainDigit = value.ToString().Any(char.IsDigit);
            return !doesContainDigit;

        }
    }
}