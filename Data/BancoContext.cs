
using VozDoPovo.Models;
using Microsoft.EntityFrameworkCore;

namespace VozDoPovo.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) :base(options) 
        {

        }

        public DbSet<DenunciaModel> Denuncias { get; set; }
        public DbSet<ComentarioModel> Comentarios { get; set; }
        //public DbSet<UserModel> Users { get; set; }
    }
}
