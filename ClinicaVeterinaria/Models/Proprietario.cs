using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicaVeterinaria.Models
{
    public class Proprietario
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nome do Proprietário")]
        public string Nome { get; set; }

        public string Cpf { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Telefone { get; set; }

        [Required]
        public string Endereco { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }
    }
}