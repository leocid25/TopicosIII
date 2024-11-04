using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicaVeterinaria.Models
{
    public class AgendamentoViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public TipoConsulta TipoConsulta { get; set; }
        public string NomeCliente { get; set; }
        public string TelefoneCliente { get; set; }
        public int VeterinarioId { get; set; }
        public string NomeVeterinario { get; set; }
        public string BackgroundColor { get; set; }
    }
}