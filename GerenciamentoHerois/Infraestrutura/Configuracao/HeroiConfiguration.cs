using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestrutura.Configuracao
{
    public class HeroiConfiguration : IEntityTypeConfiguration<Heroi>
    {
        public void Configure(EntityTypeBuilder<Heroi> builder)
        {
            builder.ToTable("Herois");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.NomeCompleto).HasColumnType("VARCHAR(120)").IsRequired();
            builder.Property(p => p.Codinome).HasColumnType("VARCHAR(30)").IsRequired();
            builder.Property(p => p.Registrado).HasColumnType("bit");
            builder.Property(p => p.Nascimento);
            builder.Property(p => p.Poder).HasConversion<string>();
            builder.Property(p => p.DataCadastro).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
        }
    }
}