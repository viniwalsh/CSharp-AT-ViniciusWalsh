using Dominio.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Contextos
{
    public class HeroiContexto : DbContext
    {
        public HeroiContexto(DbContextOptions<HeroiContexto> options) : base(options) { }

        public DbSet<Heroi> Herois { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HeroiContexto).Assembly);
        }
    }
}
