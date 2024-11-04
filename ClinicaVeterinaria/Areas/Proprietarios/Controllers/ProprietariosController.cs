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

namespace ClinicaVeterinaria.Areas.Proprietarios.Controllers
{
    
    public class ProprietariosController : Controller
    {
        private ClinicaVeterinariaContext db;
        private ApplicationUserManager _userManager;

        public ProprietariosController()
        {
            db = new ClinicaVeterinariaContext();
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        [Authorize(Roles = "Proprietario")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Veterinario, Secretario")]
        // GET: Proprietarios
        public ActionResult List()
        {
            return View(db.Proprietarios.ToList());
        }

        [Authorize(Roles = "Veterinario, Secretario")]
        // GET: Proprietarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proprietario proprietario = db.Proprietarios.Find(id);
            if (proprietario == null)
            {
                return HttpNotFound();
            }
            return View(proprietario);
        }

        [Authorize(Roles = "Veterinario, Secretario")]
        // GET: Proprietarios/Create
        public ActionResult Create(string userId)
        {
            var user = db.Users.Find(userId);
            var proprietario = new Proprietario
            {
                UserId = userId,
                Email = user.Email
            };
            ViewBag.UserId = userId; // Armazena o userId para o uso posterior
            ViewBag.Email = user.Email;
            return View();
        }

        //[Authorize(Roles = "Veterinario, Secretario")]
        // POST: Proprietarios/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(string userId, [Bind(Include = "Id,UserId,Email,Nome,Cpf,Telefone,Endereco")] Proprietario proprietario)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        proprietario.UserId = userId;
        //        proprietario.Email = db.Users.Find(userId).Email; // Associa o email do usuário ao Proprietário
        //        db.Proprietarios.Add(proprietario);
        //        db.SaveChanges();

        //        return RedirectToAction("List");
        //    }

        //    return View(proprietario);
        //}

        [Authorize(Roles = "Veterinario, Secretario, Proprietario")]
        // GET: Proprietarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proprietario proprietario = db.Proprietarios.Find(id);
            if (proprietario == null)
            {
                return HttpNotFound();
            }
            return View(proprietario);
        }

        [Authorize(Roles = "Veterinario, Secretario")]
        // POST: Proprietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string userId, [Bind(Include = "Id,UserId,Email,Nome,Cpf,Telefone,Endereco")] Proprietario proprietario)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Associa o userId e email ao Proprietário
                        proprietario.UserId = userId;
                        proprietario.Email = db.Users.Find(userId).Email;

                        // Salva o proprietário
                        db.Proprietarios.Add(proprietario);
                        await db.SaveChangesAsync();

                        // Atualiza o ApplicationUser com o ProprietarioId
                        var user = await _userManager.FindByIdAsync(userId);
                        if (user != null)
                        {
                            user.ProprietarioId = proprietario.Id;
                            var result = await _userManager.UpdateAsync(user);
                            if (!result.Succeeded)
                            {
                                transaction.Rollback();
                                ModelState.AddModelError("", "Erro ao atualizar o usuário.");
                                return View(proprietario);
                            }
                        }

                        transaction.Commit();
                        return RedirectToAction("List");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", "Ocorreu um erro ao criar o proprietário: " + ex.Message);
                        return View(proprietario);
                    }
                }
            }

            return View(proprietario);
        }

        [Authorize(Roles = "Veterinario, Secretario, Proprietario")]
        // POST: Proprietarios/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Nome,Email,Cpf,Telefone,Endereco")] Proprietario proprietario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proprietario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(proprietario);
        }

        [Authorize(Roles = "Veterinario, Secretario")]
        // GET: Proprietarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proprietario proprietario = db.Proprietarios.Find(id);
            if (proprietario == null)
            {
                return HttpNotFound();
            }
            return View(proprietario);
        }

        [Authorize(Roles = "Veterinario, Secretario")]
        // POST: Proprietarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var proprietario = await db.Proprietarios.FindAsync(id);

            if (proprietario == null)
            {
                return HttpNotFound();
            }

            try
            {
                var usuario = await _userManager.FindByIdAsync(proprietario.UserId);
                if (usuario != null)
                {
                    var result = await _userManager.DeleteAsync(usuario);
                    if (!result.Succeeded)
                    {
                        // Handle user deletion errors (e.g., add ModelState errors)
                        return View("Error");
                    }
                }

                db.Proprietarios.Remove(proprietario);
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
