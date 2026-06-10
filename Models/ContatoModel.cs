using System.ComponentModel.DataAnnotations;

namespace VozDoPovo.Models
{
    public class ContatoModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        [EmailAddress(ErrorMessage = "Sintaxe de Email inválida")]
        public string Email { get; set; }
        
        public byte Esconder { get; set; }
    }
}
