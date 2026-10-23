using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace VozDoPovo.Models
{
    [method: SetsRequiredMembers]
    public class DenunciaModel()
    {
        public int id { get; set; }
        public byte likes { get; set; } = 0;
        public byte dislikes { get; set; } = 0;
        public string? imagem { get; set; }
        public required string contexto { get; set; } = string.Empty;
        public DateTime data { get; set; }
        public bool anonimo { get; set; }

        [Required]
        [Range(-90, 90)]
        public decimal latitude { get; set; } = (decimal)-8.7608;

        [Required]
        [Range(-180, 180)]
        public decimal longitude { get; set; } = (decimal)-63.8999;

        [NotMapped]
        public virtual IFormFile? arquivoImg { get; set; }

        [NotMapped]
        public virtual List<ComentarioModel>? comentarios { get; set; }

        [NotMapped]
        public virtual int comments { get; set; } = 0;

        [NotMapped]//não é utilizado nesta classe, existe somente para ser usado no form do ComentarioModel
        public virtual string? mensagem { get; set; }
    }
    public class JsonDenuncia
    {
        public int id { get; set; }
    }
}
//create table Denuncias (
//	id int primary key identity,
//    likes tinyint not null,
//    dislikes tinyint not null,
//    imagem nvarchar(20),
//    contexto text not null,
//    data datetime not null,
//    anonimo bit,
//    latitude smallmoney not null,
//    longitude smallmoney not null
//)