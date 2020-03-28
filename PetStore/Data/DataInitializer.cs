using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetStore.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace PetStore.Data
{
    public class DataInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {
            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);
            RoleManager<IdentityRole> roleMgr = new RoleManager<IdentityRole>(roleStore);

            var role = new IdentityRole("Admin");
            roleMgr.Create(role);

            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> userMgr = new UserManager<ApplicationUser>(userStore);

            ApplicationUser user = new ApplicationUser { UserName = "test@test.com", Email = "test@test.com", DateOfBirth = new DateTime(2000, 1, 1) };
            userMgr.Create(user, "test1234");
            userMgr.AddToRole(user.Id, "Admin");

            Pet pet1 = new Pet { Name = "Test Dog", Breed = "Test Breed", Owner = null, Age = 5 };
            Pet pet2 = new Pet { Name = "Test Cat", Breed = "Test Brreed Cat", Owner = user, Age = 7 };

            context.Pets.Add(pet1);
            context.Pets.Add(pet2);

            base.Seed(context);
        }
    }
}