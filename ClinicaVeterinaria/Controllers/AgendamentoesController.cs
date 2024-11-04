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

namespace ClinicaVeterinaria.Controllers
{
    public class AgendamentoesController : Controller
    {
        private ClinicaVeterinariaContext db = new ClinicaVeterinariaContext();

        public ActionResult Index()
        {
            ViewBag.Veterinarios = new SelectList(db.Veterinarios, "Id", "Nome");
            ViewBag.TiposConsulta = new SelectList(Enum.GetValues(typeof(TipoConsulta))
                .Cast<TipoConsulta>()
                .Select(t => new { Id = (int)t, Nome = t.ToString() }), "Id", "Nome");

            // Define o role do usuário para a view
            if (User.IsInRole("Proprietario"))
            {
                ViewBag.UserRole = "Proprietario";
            }
            else if (User.IsInRole("Curioso"))
            {
                ViewBag.UserRole = "Curioso";
            }
            else
            {
                ViewBag.UserRole = "";
            }

            return View();
        }

        public JsonResult ObterAgendamentos(int? veterinarioId)
        {
            var agendamentosQuery = db.Agendamentos
                .Include(a => a.Veterinario)
                .AsQueryable();

            if (veterinarioId.HasValue)
            {
                agendamentosQuery = agendamentosQuery.Where(a => a.VeterinarioId == veterinarioId.Value);
            }

            var agendamentos = agendamentosQuery
                .Select(a => new
                {
                    a.Id,
                    a.NomeCliente,
                    a.TelefoneCliente,
                    a.TipoConsulta,
                    a.Descricao,
                    a.DataInicio,
                    a.DataFim,
                    a.VeterinarioId,
                    NomeVeterinario = a.Veterinario.Nome,
                    a.CorFundo
                })
                .ToList();

            var eventos = agendamentos.Select(a => new AgendamentoViewModel
            {
                Id = a.Id,
                Title = $"{a.NomeCliente} - {a.TipoConsulta}",
                NomeCliente = a.NomeCliente, // Campo individual para NomeCliente
                TelefoneCliente = a.TelefoneCliente, // Campo individual para TelefoneCliente
                TipoConsulta = a.TipoConsulta, // Campo individual para TipoConsulta
                Description = a.Descricao, // Campo individual para Descrição/Observações
                Start = a.DataInicio.ToString("s"),
                End = a.DataFim.ToString("s"),
                VeterinarioId = a.VeterinarioId,
                BackgroundColor = a.CorFundo
            }).ToList();

            return Json(eventos, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Veterinario, Secretario")]
        [HttpPost]
        public JsonResult SalvarAgendamento(AgendamentoViewModel model)
        {
            try
            {
                var dataInicio = DateTime.Parse(model.Start);
                var dataFim = DateTime.Parse(model.End);

                // Verifica conflitos para o veterinário específico
                var conflito = db.Agendamentos
                    .Where(a => a.VeterinarioId == model.VeterinarioId)
                    .Any(a => (model.Id == 0 || a.Id != model.Id) &&
                        (
                            (dataInicio >= a.DataInicio && dataInicio < a.DataFim) ||
                            (dataFim > a.DataInicio && dataFim <= a.DataFim) ||
                            (a.DataInicio >= dataInicio && a.DataInicio < dataFim)
                        ));

                if (conflito)
                {
                    return Json(new { success = false, message = "Este veterinário já possui um agendamento neste horário" });
                }

                if (model.Id == 0)
                {
                    var agendamento = new Agendamento
                    {
                        Titulo = model.Title,
                        Descricao = model.Description,
                        DataInicio = dataInicio,
                        DataFim = dataFim,
                        TipoConsulta = model.TipoConsulta,
                        NomeCliente = model.NomeCliente,
                        TelefoneCliente = model.TelefoneCliente,
                        VeterinarioId = model.VeterinarioId,
                        CorFundo = model.BackgroundColor,
                        DataCriacao = DateTime.Now
                    };

                    db.Agendamentos.Add(agendamento);
                }
                else
                {
                    var agendamento = db.Agendamentos.Find(model.Id);
                    if (agendamento != null)
                    {
                        agendamento.Titulo = model.Title;
                        agendamento.Descricao = model.Description;
                        agendamento.DataInicio = dataInicio;
                        agendamento.DataFim = dataFim;
                        agendamento.TipoConsulta = model.TipoConsulta;
                        agendamento.NomeCliente = model.NomeCliente;
                        agendamento.TelefoneCliente = model.TelefoneCliente;
                        agendamento.VeterinarioId = model.VeterinarioId;
                        agendamento.CorFundo = model.BackgroundColor;
                    }
                }

                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro ao salvar: " + ex.Message });
            }
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
