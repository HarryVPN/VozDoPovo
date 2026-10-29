using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using VozDoPovo.Data;

namespace VozDoPovo.Models
{
    [method: SetsRequiredMembers]
    public class PerfilModel
    {
        public int id { get; set; }

        [StringLength(25, MinimumLength = 3)]
        public required string nome { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Pass word is required")]
        [StringLength(25, MinimumLength = 3)]
        [DataType(DataType.Password)]
        public required string senha { get; set; } = string.Empty;

        public IdCargos cargo { get; set; }

        public DateTime criada { get; set; }

        public DateTime ultimoLogin { get; set; }

        public string? foto { get; set; }
    }
}
/*
create table Perfis (
	id int primary key identity,
    nome nvarchar(25) not null,
    senha varchar(25) not null,
    cargo tinyint not null,
    criada datetime not null,
    ultimoLogin datetime not null,
    foto nvarchar(20)
)
*/