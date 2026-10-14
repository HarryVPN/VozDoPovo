using System.ComponentModel.DataAnnotations;

namespace VozDoPovo.Models
{
    public class ContatoModel
    {
        public int Id { get; set; }

        public required string Nome { get; set; }

        [EmailAddress(ErrorMessage = "Sintaxe de Email inválida")]
        public required string Email { get; set; }
        
        public byte Esconder { get; set; }
    }
}
