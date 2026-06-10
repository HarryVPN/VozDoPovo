using VozDoPovo.Data;
using System.ComponentModel.DataAnnotations;

namespace VozDoPovo.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        [EmailAddress(ErrorMessage = "Sintaxe de Email inválida")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Cargo inválido")]
        public IDsCargos Cargo { get; set; }
        public string? Senha { get; set; }
        public DateTime? Cadastro { get; set; }

        public DateTime? Atualizado { get; set; }
    }
}
