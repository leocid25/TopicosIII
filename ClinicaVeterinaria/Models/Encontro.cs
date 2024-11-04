using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicaVeterinaria.Models
{
    public class Encontro
    {
        public int Id { get; set; }
        [Display(Name = "Nome do Encontro")]
        public string Nome { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime HoraInicio { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? HoraTermino { get; set; }
        [Display(Name = "Tipo de Consulta")]
        public TipoConsulta TipoConsulta { get; set; }
        [Display(Name = "Valor da Consulta")]
        public double? ValorPagoConsulta { get; set; }
        public string Descricao { get; set; }
        [Display(Name = "Nome do Animal")]
        public int PetId { get; set; }
        [Display(Name = "Nome do Veterinário")]
        public int VeterinarioId { get; set; }
        public virtual Pet Pet { get; set; }
        public virtual Veterinario Veterinario { get; set; }
        public virtual ICollection<Tratamento> Tratamentos { get; set; }
    }
}