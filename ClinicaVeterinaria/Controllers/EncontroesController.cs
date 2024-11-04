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
using System.Globalization;

namespace ClinicaVeterinaria.Controllers
{
    public class EncontroesController : Controller
    {
        private ClinicaVeterinariaContext db = new ClinicaVeterinariaContext();

        [Authorize(Roles = "Veterinario, Secretario, Proprietario")]
        public ActionResult Index(int? petId, int? veterinarioId)
        {
            var encontros = db.Encontros.Include(e => e.Pet).Include(e => e.Veterinario);

            // Aplica os filtros se os parâmetros forem válidos
            if (petId.HasValue)
            {
                encontros = encontros.Where(e => e.PetId == petId.Value);
            }
            if (veterinarioId.HasValue)
            {
                encontros = encontros.Where(e => e.VeterinarioId == veterinarioId.Value);
            }

            ViewBag.TipoConsulta = new SelectList(
                Enum.GetNames(typeof(TipoConsulta)));

            return View(encontros.ToList());
        }

        [Authorize(Roles = "Veterinario, Secretario, Proprietario")]
        public ActionResult EncontrosPorVeterinario(int? veterinarioId)
        {
            var encontros = db.Encontros.Include(e => e.Veterinario);

            // Aplica os filtros se os parâmetros forem válidos
            if (veterinarioId.HasValue)
            {
                encontros = encontros.Where(e => e.VeterinarioId == veterinarioId.Value);
            }

            ViewBag.TipoConsulta = new SelectList(
                Enum.GetNames(typeof(TipoConsulta)));

            return View(encontros.ToList());
        }

        // GET: Encontroes/Details/5
        [Authorize(Roles = "Veterinario, Secretario, Proprietario")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encontro encontro = db.Encontros.Find(id);
            if (encontro == null)
            {
                return HttpNotFound();
            }
            return View(encontro);
        }

        // GET: Encontroes/Create
        [Authorize(Roles = "Veterinario")]
        public ActionResult Create()
        {
            // Criando a lista de tipos de consulta
            ViewBag.TipoConsulta = new SelectList(
                from TipoConsulta e in Enum.GetValues(typeof(TipoConsulta))
                select new
                {
                    ID = e,
                    Name = e.ToString()
                }, "ID", "Name");
            ViewBag.PetId = new SelectList(db.Pets, "Id", "Nome");
            ViewBag.VeterinarioId = new SelectList(db.Veterinarios, "Id", "Nome");
            return View();
        }

        // POST: Encontroes/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Veterinario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,HoraInicio,HoraTermino,TipoConsulta,ValorPagoConsulta,Descricao,PetId,VeterinarioId")] Encontro encontro)
        {
            if (ModelState.IsValid)
            {
                db.Encontros.Add(encontro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Recarregar as listas em caso de erro
            ViewBag.TipoConsulta = new SelectList(
                from TipoConsulta e in Enum.GetValues(typeof(TipoConsulta))
                select new
                {
                    ID = e,
                    Name = e.ToString()
                }, "ID", "Name");
            ViewBag.PetId = new SelectList(db.Pets, "Id", "Nome", encontro.PetId);
            ViewBag.VeterinarioId = new SelectList(db.Veterinarios, "Id", "Nome", encontro.VeterinarioId);
            return View(encontro);
        }

        //GET: Encontroes/Edit/5
        [Authorize(Roles = "Veterinario")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Usando Include para carregar as entidades relacionadas se necessário
            Encontro encontro = db.Encontros
                .Include(e => e.Pet)
                .Include(e => e.Veterinario)
                .FirstOrDefault(e => e.Id == id);

            if (encontro == null)
            {
                return HttpNotFound();
            }

            // Formata as datas para o formato "yyyy-MM-ddTHH:mm"
            ViewBag.HoraInicioFormatted = string.Format("{0:yyyy-MM-ddTHH:mm}", encontro.HoraInicio);
            ViewBag.HoraTerminoFormatted = string.Format("{0:yyyy-MM-ddTHH:mm}", encontro.HoraTermino);

            // Obtém todos os valores do enum
            var tiposConsulta = Enum.GetValues(typeof(TipoConsulta)).Cast<TipoConsulta>().ToList();

            // Cria uma lista para armazenar os SelectListItems
            var tiposConsultaList = tiposConsulta.Select(e => new SelectListItem
            {
                Value = ((int)e).ToString(),
                Text = e.ToString(),
                Selected = (int)e == (int)encontro.TipoConsulta
            }).ToList();

            // Reordena para que o selecionado fique em primeiro lugar
            var selecionado = tiposConsultaList.FirstOrDefault(x => x.Selected);
            if (selecionado != null)
            {
                tiposConsultaList.Remove(selecionado); // Remove o selecionado
                tiposConsultaList.Insert(0, selecionado); // Insere o selecionado no início
            }

            // Cria a SelectList com o valor selecionado no início
            ViewBag.TipoConsulta = new SelectList(tiposConsultaList, "Value", "Text");

            ViewBag.PetId = new SelectList(db.Pets, "Id", "Nome", encontro.PetId);
            ViewBag.VeterinarioId = new SelectList(db.Veterinarios, "Id", "Nome", encontro.VeterinarioId);
            return View(encontro);
        }

        //POST: Encontroes/Edit/5
        //Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar.
        //Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Veterinario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,HoraInicio,HoraTermino,TipoConsulta,ValorPagoConsulta,Descricao,PetId,VeterinarioId")] Encontro encontro)
        {
            if (ModelState.IsValid)
            {

                // Carrega o encontro original do banco
                var encontroOriginal = db.Encontros.Find(encontro.Id);

                // Atualiza apenas os campos que vieram do formulário
                encontroOriginal.Nome = encontro.Nome;
                encontroOriginal.HoraInicio = encontro.HoraInicio;
                encontroOriginal.HoraTermino = encontro.HoraTermino;
                encontroOriginal.TipoConsulta = encontro.TipoConsulta;
                encontroOriginal.ValorPagoConsulta = encontro.ValorPagoConsulta;
                encontroOriginal.Descricao = encontro.Descricao;
                encontroOriginal.PetId = encontro.PetId;
                encontroOriginal.VeterinarioId = encontro.VeterinarioId;

                // Marca o encontroOriginal como modificado
                db.Entry(encontroOriginal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Recarrega os dados para o formulário em caso de erro
            var tiposConsulta = Enum.GetValues(typeof(TipoConsulta))
                .Cast<TipoConsulta>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString(),
                    Selected = e == encontro.TipoConsulta
                });

            ViewData["TipoConsulta"] = new SelectList(tiposConsulta, "Value", "Text", (int)encontro.TipoConsulta);

            ViewBag.PetId = new SelectList(db.Pets, "Id", "Nome", encontro.PetId);
            ViewBag.VeterinarioId = new SelectList(db.Veterinarios, "Id", "Nome", encontro.VeterinarioId);
            return View(encontro);
        }

        // GET: Encontroes/Delete/5
        [Authorize(Roles = "Veterinario")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encontro encontro = db.Encontros.Find(id);
            if (encontro == null)
            {
                return HttpNotFound();
            }
            return View(encontro);
        }

        //POST: Encontroes/Delete/5
        [Authorize(Roles = "Veterinario")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Encontro encontro = db.Encontros.Find(id);
            db.Encontros.Remove(encontro);
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
