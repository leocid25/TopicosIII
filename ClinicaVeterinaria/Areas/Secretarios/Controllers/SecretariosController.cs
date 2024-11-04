using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ClinicaVeterinaria.Data;
using ClinicaVeterinaria.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ClinicaVeterinaria.Areas.Secretarios.Controllers
{
    
    public class SecretariosController : Controller
    {
        private ClinicaVeterinariaContext db;
        private ApplicationUserManager _userManager;

        public SecretariosController()
        {
            db = new ClinicaVeterinariaContext();
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        [Authorize(Roles = "Secretario")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Secretarios
        public ActionResult List()
        {
            return View(db.Secretarios.ToList());
        }

        [Authorize(Roles = "Veterinario, Secretario")]
        // GET: Secretarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Secretario secretario = db.Secretarios.Find(id);
            if (secretario == null)
            {
                return HttpNotFound();
            }
            return View(secretario);
        }

        [Authorize(Roles = "Veterinario")]
        // GET: Secretarios/Create
        public ActionResult Create(string userId)
        {
            var user = db.Users.Find(userId);
            var secretario = new Secretario
            {
                UserId = userId,
                Email = user.Email
            };
            ViewBag.UserId = userId; // Armazena o userId para o uso posterior
            ViewBag.Email = user.Email;
            return View();
        }

        [Authorize(Roles = "Veterinario")]
        // POST: Secretarios/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string userId, [Bind(Include = "id,UserId,Email,Nome,Cpf,Status,Telefone,Endereco")] Secretario secretario)
        {
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "O ID do usuário não foi encontrado.");
                return View(secretario);
            }
            if (ModelState.IsValid)
            {
                secretario.UserId = userId;
                if (db.Users == null)
                {
                    ModelState.AddModelError("", "A tabela de usuários não está disponível.");
                    return View(secretario);
                }
                var user = db.Users.Find(userId);
                if (user == null)
                {
                    ModelState.AddModelError("", "Usuário não encontrado.");
                    return View(secretario);
                }
                secretario.Email = user.Email;
                db.Secretarios.Add(secretario);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(secretario);
        }

        [Authorize(Roles = "Veterinario, Secretario")]
        // GET: Secretarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Secretario secretario = db.Secretarios.Find(id);
            if (secretario == null)
            {
                return HttpNotFound();
            }
            return View(secretario);
        }

        [Authorize(Roles = "Veterinario, Secretario")]
        // POST: Secretarios/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Nome,Cpf,Status,Email,Telefone,Endereco")] Secretario secretario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(secretario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(secretario);
        }

        [Authorize(Roles = "Veterinario")]
        // GET: Secretarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Secretario secretario = db.Secretarios.Find(id);
            if (secretario == null)
            {
                return HttpNotFound();
            }
            return View(secretario);
        }

        [Authorize(Roles = "Veterinario")]
        // POST: Secretarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var secretario = await db.Secretarios.FindAsync(id);

            if (secretario == null)
            {
                return HttpNotFound();
            }

            try
            {
                var usuario = await _userManager.FindByIdAsync(secretario.UserId);
                if (usuario != null)
                {
                    var result = await _userManager.DeleteAsync(usuario);
                    if (!result.Succeeded)
                    {
                        // Handle user deletion errors (e.g., add ModelState errors)
                        return View("Error");
                    }
                }

                db.Secretarios.Remove(secretario);
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
