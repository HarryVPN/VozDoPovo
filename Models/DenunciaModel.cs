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

        public double latitude { get; set; } = -8.769666;

        public double longitude { get; set; } = -63.884296;

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
//    latitude float not null,
//    longitude float not null
//)