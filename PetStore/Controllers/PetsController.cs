using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PetStore.Models;
using PetStore.Models.ViewModel;
using System.Security.Claims;
using PetStore.Services;
using PetStore.Interfaces;


namespace PetStore.Controllers
{
    [Authorize(Roles ="Admin")]
    public class PetsController : Controller
    {
        private ApplicationDbContext db;
        private IPetService petService;

        public PetsController(IPetService petService, ApplicationDbContext context)
        {
            this.petService = petService;
            this.db = context;
        }

        [Authorize]
        public ActionResult MyPets()
        {
            //Filter out all pets that do not belong to the user
            var userName = User.Identity.Name;
            List<Pet> myPets = db.Pets.Include(x => x.Owner).Where(pet => pet.Owner.UserName == userName).ToList();
            return View("MyPet",myPets);
        }

        public ActionResult GetPetsByBreed(string breed)
        {
            PetService petService = new PetService(db);
            var model = petService.GetPetsByBreed(breed);
            return PartialView("_PetListPartial", model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Adopt(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }

            return View(pet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Adopt(Pet toAdopt)
        {

            var userName = User.Identity.Name;
            var user = db.Users.Include(x => x.Pets).Where(x => x.UserName == userName).First();
            var pet = db.Pets.Find(toAdopt.ID);

            //Query the users claims and convert it to a DateTime
            var claimUser = (ClaimsPrincipal)User;

            var dateOfBirth = Convert.ToDateTime(claimUser.Claims
                .Where(claim => claim.Type == System.IdentityModel.Claims.ClaimTypes.DateOfBirth)
                .First()
                .Value);

            var age = DateTime.Now.Year - user.DateOfBirth.Year;
            if (user.DateOfBirth > DateTime.Today.AddYears(-age))
                age = age - 1;

            if (age >= 18)
            {
                //user.Pets.Add(pet);
                pet.Owner = user;
                db.SaveChanges();
            }


            return RedirectToAction("MyPets");
        }

        // GET: Pets
        [AllowAnonymous]
        public ActionResult Index()
        {
            //Filter out all pets that have an owner by populating the 
            //Owner child propery and filtering using .Where
            return View(db.Pets.Select(p => p.Breed).Distinct().ToList());
        }

        // GET: Pets/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        // GET: Pets/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var model = new CreatePetViewModel();
            model.DropDownBreed = db.Pets.Select(p => p.Breed).Distinct().Select(p => new SelectListItem { Value = p, Text = p }).ToList();
            return View(model);
        }

        // POST: Pets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Breed,Name,isMale,Age")] Pet pet)
        {
            if (ModelState.IsValid)
            {
                db.Pets.Add(pet);
                db.SaveChanges();
                return Json(new { text = "Success" }, JsonRequestBehavior.AllowGet);
            }
            

            var model = new EditPetViewModel();
            model.ID = pet.ID;
            model.Name = pet.Name;
            model.Age = pet.Age;
            model.Breed = pet.Breed;
            model.isMale = pet.isMale;

            model.DropDownBreed = db.Pets.Select(p => p.Breed).Distinct().Select(p => new SelectListItem { Value = p, Text = p, Selected = (p == pet.Breed) }).ToList();

            return Json(new { text = "Failed" }, JsonRequestBehavior.DenyGet);
        }

        // GET: Pets/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }

            var model = new EditPetViewModel();
            model.ID = pet.ID;
            model.Name = pet.Name;
            model.Age = pet.Age;
            model.Breed = pet.Breed;
            model.isMale = pet.isMale;

            model.DropDownBreed = db.Pets.Select(p => p.Breed).Distinct().Select(p => new SelectListItem { Value = p, Text = p, Selected = (p == pet.Breed) }).ToList();
            return View(model);
        }

        // POST: Pets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ID,Breed,Name,isMale,Age")] Pet pet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pet).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { text = "Success" }, JsonRequestBehavior.AllowGet); 
            }
            return Json(new { text = "Failed" }, JsonRequestBehavior.DenyGet);
        }

        // GET: Pets/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        // POST: Pets/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pet pet = db.Pets.Find(id);
            db.Pets.Remove(pet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
