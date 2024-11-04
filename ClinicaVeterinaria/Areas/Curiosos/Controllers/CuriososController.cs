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

namespace ClinicaVeterinaria.Areas.Curiosos.Controllers
{
    public class CuriososController : Controller
    {
        private ClinicaVeterinariaContext db = new ClinicaVeterinariaContext();

        // GET: Curiosos/Curiosoes
        public ActionResult Index()
        {
            return View(db.Curiosoes.ToList());
        }

        // GET: Curiosos/Curiosoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curioso curioso = db.Curiosoes.Find(id);
            if (curioso == null)
            {
                return HttpNotFound();
            }
            return View(curioso);
        }

        // GET: Curiosos/Curiosoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Curiosos/Curiosoes/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId")] Curioso curioso)
        {
            if (ModelState.IsValid)
            {
                db.Curiosoes.Add(curioso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(curioso);
        }

        // GET: Curiosos/Curiosoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curioso curioso = db.Curiosoes.Find(id);
            if (curioso == null)
            {
                return HttpNotFound();
            }
            return View(curioso);
        }

        // POST: Curiosos/Curiosoes/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId")] Curioso curioso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(curioso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(curioso);
        }

        // GET: Curiosos/Curiosoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curioso curioso = db.Curiosoes.Find(id);
            if (curioso == null)
            {
                return HttpNotFound();
            }
            return View(curioso);
        }

        // POST: Curiosos/Curiosoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Curioso curioso = db.Curiosoes.Find(id);
            db.Curiosoes.Remove(curioso);
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
