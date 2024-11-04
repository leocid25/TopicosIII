using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using ClinicaVeterinaria.Data;
using ClinicaVeterinaria.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ClinicaVeterinaria.Areas.Veterinarios.Controllers
{
    public class VeterinariosController : Controller
    {
        private ClinicaVeterinariaContext db;
        private ApplicationUserManager _userManager;

        public VeterinariosController()
        {
            db = new ClinicaVeterinariaContext();
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        [Authorize(Roles = "Veterinario")]
        // GET: Veterinarios
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Veterinario")]
        public ActionResult EncontrosPorVeterinario(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Redireciona para a ação Index do EncontroesController com o parâmetro de filtro
            return RedirectToAction("Index", "Encontroes", new { veterinarioId = id });
        }

        public ActionResult List()
        {
            return View(db.Veterinarios.ToList());
        }

        [Authorize(Roles = "Veterinario, Secretario, Proprietario")]
        // GET: Veterinarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veterinario veterinario = db.Veterinarios.Find(id);
            if (veterinario == null)
            {
                return HttpNotFound();
            }
            return View(veterinario);
        }

        [Authorize(Roles = "Veterinario")]
        // GET: Veterinarios/Create
        public ActionResult Create(string userId)
        {
            var user = db.Users.Find(userId);
            var veterinario = new Veterinario
            {
                UserId = userId,
                Email = user?.Email
            };
            ViewBag.UserId = userId;
            ViewBag.Email = user?.Email;
            return View(veterinario);
        }

        [Authorize(Roles = "Veterinario")]
        // POST: Veterinarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string userId, [Bind(Include = "Id,UserId,Nome,Email,Telefone,Cpf,Status,Endereco,Especializacao,CRMV")] Veterinario veterinario)
        {
            if (ModelState.IsValid)
            {
                veterinario.UserId = userId;
                veterinario.Email = db.Users.Find(userId)?.Email;
                db.Veterinarios.Add(veterinario);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(veterinario);
        }

        [Authorize(Roles = "Veterinario")]
        // GET: Veterinarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veterinario veterinario = db.Veterinarios.Find(id);
            if (veterinario == null)
            {
                return HttpNotFound();
            }
            return View(veterinario);
        }

        [Authorize(Roles = "Veterinario")]
        // POST: Veterinarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Nome,Email,Telefone,Cpf,Status,Endereco,Especializacao,CRMV")] Veterinario veterinario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(veterinario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(veterinario);
        }

        [Authorize(Roles = "Veterinario")]
        // GET: Veterinarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veterinario veterinario = db.Veterinarios.Find(id);
            if (veterinario == null)
            {
                return HttpNotFound();
            }
            return View(veterinario);
        }

        [Authorize(Roles = "Veterinario")]
        // POST: Veterinarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var veterinario = await db.Veterinarios.FindAsync(id);

            if (veterinario == null)
            {
                return HttpNotFound();
            }

            try
            {
                var usuario = await _userManager.FindByIdAsync(veterinario.UserId);
                if (usuario != null)
                {
                    var result = await _userManager.DeleteAsync(usuario);
                    if (!result.Succeeded)
                    {
                        // Handle user deletion errors (e.g., add ModelState errors)
                        return View("Error");
                    }
                }

                db.Veterinarios.Remove(veterinario);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle any other potential errors during deletion
                ModelState.AddModelError("", ex.Message);
                return View("Error");
            }

            return RedirectToAction("List");
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
