using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicaVeterinaria.Models
{
    public class Pet
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nome do Animal")]
        public string Nome { get; set; }

        [Required]
        public string Especie { get; set; }

        public string Raca { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        public string Genero { get; set; }

        public int ProprietarioId { get; set; }

        public virtual Proprietario Proprietario { get; set; }
        public virtual ICollection<Encontro> Encontros { get; set; }
        public virtual ICollection<Tratamento> Tratamentos { get; set; }
    }

}