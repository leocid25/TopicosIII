using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicaVeterinaria.Models
{
    public class Tratamento
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime HoraInicio { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? HoraTermino { get; set; }
        [Display(Name = "Status do Tratamento")]
        public Status? Status { get; set; }
        [Display(Name = "Valor do Tratamento")]
        public double? ValorPagoTratamento { get; set; }
        public int PetId { get; set; }
        public int EncontroId { get; set; }
        public virtual Pet Pet { get; set; }
        public virtual Encontro Encontro { get; set; }
        public virtual ICollection<Receita> Medicacoes { get; set; }


        public Tratamento() { Medicacoes = new List<Receita>(); }
    }
}