using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicaVeterinaria.Models
{
    public class Receita
    {
        public int Id { get; set; }
        [Display(Name = "Nome do Medicamento")]
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Dosagem { get; set; }
        public double? Preco { get; set; }
        public virtual ICollection<Tratamento> Tratamentos { get; set; }
    }
}