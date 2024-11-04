using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicaVeterinaria.Data;
using ClinicaVeterinaria.Models;

namespace ClinicaVeterinaria.Controllers
{
    public class TratamentoesController : Controller
    {
        private ClinicaVeterinariaContext db = new ClinicaVeterinariaContext();

        // GET: Tratamentoes
        [Authorize(Roles = "Veterinario, Secretario, Proprietario")]
        public ActionResult Index(int? encontroId)
        {
            var tratamentos = db.Tratamentos.Include(t => t.Encontro);

            if (encontroId.HasValue)
            {
                tratamentos = tratamentos.Where(t => t.EncontroId == encontroId);
            }
                    
            ViewBag.EncontroId = encontroId;
          
            return View(tratamentos.ToList());
        }

        // GET: Tratamentoes/GetEncontrosPorPet
        [Authorize(Roles = "Veterinario, Secretario, Proprietario")]
        public JsonResult GetEncontrosPorPet(int petId)
        {
            var encontros = db.Encontros
                .Where(e => e.PetId == petId)
                .Select(e => new { e.Id, e.Nome }) // Selecione os campos necessários
                .ToList();

            return Json(encontros, JsonRequestBehavior.AllowGet);
        }

        // GET: Tratamentoes/Details/5
        [Authorize(Roles = "Veterinario, Secretario, Proprietario")]
        public ActionResult Details(int? id, int? encontroId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tratamento tratamento = db.Tratamentos.Find(id);
            if (tratamento == null)
            {
                return HttpNotFound();
            }
            ViewBag.EncontroId = encontroId;
            return View(tratamento);
        }

        // GET: Tratamentoes/Create
        [Authorize(Roles = "Veterinario")]
        public ActionResult Create(int? encontroId)
        {
          
            //ViewBag.EncontroId = new SelectList(db.Encontros, "Id", "Nome");
            //ViewBag.PetId = new SelectList(db.Pets, "Id", "Nome");
            //return View();

            // Carregar os objetos completos do banco de dados
            var encontro = encontroId.HasValue ? db.Encontros.Find(encontroId.Value) : null;
            ViewBag.Encontro = encontro;
            var petId = encontro.PetId;
            var pet = db.Pets.Find(petId);
            ViewBag.Pet = pet;

            // Verificar se o pet foi encontrado
            if (encontro == null)
            {
                // Exibir uma mensagem ou redirecionar para outra página
                return View("EncontroNaoEncontrado");
            }


            // Verificar se o pet foi encontrado
            if (pet == null)
            {
                // Exibir uma mensagem ou redirecionar para outra página
                return View("PetNaoEncontrado");
            }
            
            return View();
        }

        [Authorize(Roles = "Veterinario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descricao,HoraInicio,HoraTermino,Status,ValorPagoTratamento,PetId,EncontroId")] Tratamento tratamento, List<Receita> receitas)
        {
            if (ModelState.IsValid)
            {
                // Associa cada receita ao tratamento
                foreach (var receita in receitas)
                {
                    if (!string.IsNullOrEmpty(receita.Nome)) // Verifique se o nome da receita está preenchido
                    {
                        tratamento.Medicacoes.Add(receita);
                        db.Medicacoes.Add(receita); // Salva a receita na tabela Receita
                    }
                }

                db.Tratamentos.Add(tratamento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EncontroId = new SelectList(db.Encontros, "Id", "Nome", tratamento.EncontroId);
            ViewBag.PetId = new SelectList(db.Pets, "Id", "Nome", tratamento.PetId);
            return View(tratamento);
        }




        // POST: Tratamentoes/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Veterinario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descricao,HoraInicio,HoraTermino,Status,ValorPagoTratamento,PetId,EncontroId")] Tratamento tratamento)
        {
            if (ModelState.IsValid)
            {
                db.Tratamentos.Add(tratamento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EncontroId = new SelectList(db.Encontros, "Id", "Nome", tratamento.EncontroId);
            ViewBag.PetId = new SelectList(db.Pets, "Id", "Nome", tratamento.PetId);
            return View(tratamento);
        }

        //GET: Tratamentoes/Edit/5
        [Authorize(Roles = "Veterinario")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tratamento tratamento = db.Tratamentos.Find(id);
            if (tratamento == null)
            {
                return HttpNotFound();
            }

            // Formata as datas para o formato "yyyy-MM-ddTHH:mm"
            ViewBag.HoraInicioFormatted = string.Format("{0:yyyy-MM-ddTHH:mm}", tratamento.HoraInicio);
            ViewBag.HoraTerminoFormatted = string.Format("{0:yyyy-MM-ddTHH:mm}", tratamento.HoraTermino);

            ViewBag.EncontroId = new SelectList(db.Encontros, "Id", "Nome", tratamento.EncontroId);
            ViewBag.PetId = new SelectList(db.Pets, "Id", "Nome", tratamento.PetId);
            return View(tratamento);
        }

        //POST: Tratamentoes/Edit/5
        //Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar.
        //Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Veterinario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descricao,HoraInicio,HoraTermino,Status,ValorPagoTratamento,PetId,EncontroId")] Tratamento tratamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tratamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EncontroId = new SelectList(db.Encontros, "Id", "Nome", tratamento.EncontroId);
            ViewBag.PetId = new SelectList(db.Pets, "Id", "Nome", tratamento.PetId);
            return View(tratamento);
        }

        //GET: Tratamentoes/Delete/5
        [Authorize(Roles = "Veterinario")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tratamento tratamento = db.Tratamentos.Find(id);
            if (tratamento == null)
            {
                return HttpNotFound();
            }
            return View(tratamento);
        }

        //POST: Tratamentoes/Delete/5
        [Authorize(Roles = "Veterinario")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tratamento tratamento = db.Tratamentos.Find(id);
            db.Tratamentos.Remove(tratamento);
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
