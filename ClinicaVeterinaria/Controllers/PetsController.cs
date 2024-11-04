using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicaVeterinaria.Data;
using ClinicaVeterinaria.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace ClinicaVeterinaria.Controllers
{
    public class PetsController : Controller
    {
        private ClinicaVeterinariaContext db = new ClinicaVeterinariaContext();

        // GET: Pets
        [Authorize(Roles = "Veterinario, Secretario, Proprietario")]
        public ActionResult Index()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ClinicaVeterinariaContext()));
            var currentUser = userManager.FindById(User.Identity.GetUserId());

            // Verifica se o usuário atual está no role Proprietario
            if (User.IsInRole("Proprietario"))
            {
                // Busca apenas os pets do proprietário logado
                var proprietarioId = currentUser.ProprietarioId; // Assumindo que você tem essa propriedade no ApplicationUser
                var petsDoProprietario = db.Pets
                    .Include(p => p.Proprietario)
                    .Where(p => p.ProprietarioId == proprietarioId)
                    .ToList();

                return View(petsDoProprietario);
            }

            // Para Veterinario e Secretario, mostra todos os pets
            var pets = db.Pets.Include(p => p.Proprietario).ToList();
            return View(pets);
        }

        public ActionResult EncontrosPorPet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Redireciona para a ação Index do EncontroesController com o parâmetro de filtro
            return RedirectToAction("Index", "Encontroes", new { petId = id });
        }

        [Authorize(Roles = "Veterinario, Secretario, Proprietario")]
        // GET: Pets/Details/5
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

            // Verifica se o usuário é Proprietario e se o pet pertence a ele
            if (User.IsInRole("Proprietario"))
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ClinicaVeterinariaContext()));
                var currentUser = userManager.FindById(User.Identity.GetUserId());
                var proprietarioId = currentUser.ProprietarioId;

                if (pet.ProprietarioId != proprietarioId)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                }
            }

            return View(pet);
        }

        [Authorize(Roles = "Veterinario, Secretario")]
        // GET: Pets/Create
        public ActionResult Create()
        {
            ViewBag.ProprietarioId = new SelectList(db.Proprietarios, "Id", "Nome");
            return View();
        }

        [Authorize(Roles = "Veterinario, Secretario")]
        // POST: Pets/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Especie,Raca,DataNascimento,Genero,ProprietarioId")] Pet pet)
        {
            if (ModelState.IsValid)
            {
                db.Pets.Add(pet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProprietarioId = new SelectList(db.Proprietarios, "Id", "Nome", pet.ProprietarioId);
            return View(pet);
        }

        // GET: Pets/Edit/5
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
            ViewBag.ProprietarioId = new SelectList(db.Proprietarios, "Id", "Nome", pet.ProprietarioId);
            return View(pet);
        }

        [Authorize(Roles = "Veterinario, Secretario")]
        // POST: Pets/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Especie,Raca,DataNascimento,Genero,ProprietarioId")] Pet pet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProprietarioId = new SelectList(db.Proprietarios, "Id", "Nome", pet.ProprietarioId);
            return View(pet);
        }

        [Authorize(Roles = "Veterinario, Secretario")]
        // GET: Pets/Delete/5
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

        [Authorize(Roles = "Veterinario, Secretario")]
        // POST: Pets/Delete/5
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
