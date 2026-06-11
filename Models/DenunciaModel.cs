using System.ComponentModel.DataAnnotations.Schema;

namespace VozDoPovo.Models
{
    public class DenunciaModel
    {
        public int id { get; set; }
        public string? imagem { get; set; }
        public string contexto { get; set; }

        public DateTime data { get; set; }
        
        [NotMapped] 
        public virtual IFormFile? arquivoImg { get; set; }
    }
    public class JsonDenuncia
    {
        public int id { get; set; }
    }
}
