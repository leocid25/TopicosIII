using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicaVeterinaria.Models
{
    public class Agendamento
    {
        public int Id { get; set; }
        [Required]
        public DateTime DataInicio { get; set; }
        [Required]
        public DateTime DataFim { get; set; }
        public TipoConsulta TipoConsulta { get; set; }
        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }
        [StringLength(500)]
        public string Descricao { get; set; }
        [Required]
        public string NomeCliente { get; set; }
        [Required]
        [Phone]
        public string TelefoneCliente { get; set; }
        public int VeterinarioId { get; set; }
        public virtual Veterinario Veterinario { get; set; }
        [StringLength(7)]
        public string CorFundo { get; set; } = "#3788d8";
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}