using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace VozDoPovo.Models
{
    [method: SetsRequiredMembers]
    public class ComentarioModel()
    {
        public int id { get; set; }

        public int idPost { get; set; }
        public required string mensagem { get; set; } = string.Empty;
        public DateTime data { get; set; }
    }
}
//create table Comentarios (
//	id int primary key identity,
//    idPost int not null,
//    mensagem text not null,
//    data datetime not null,
//    CONSTRAINT fkComentario FOREIGN KEY (idPost) REFERENCES Denuncias(id)
//)