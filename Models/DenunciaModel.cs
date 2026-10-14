//using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.HttpResults;
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

        [NotMapped]
        public virtual IFormFile? arquivoImg { get; set; }
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
//)