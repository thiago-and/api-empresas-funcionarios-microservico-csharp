using CadastroEmpresas.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroEmpresas.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Empresa> Empresas => Set<Empresa>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var empresa = modelBuilder.Entity<Empresa>();

            empresa.ToTable("Empresas");
            empresa.HasKey(e => e.Id);
            empresa.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(200);

            empresa.Property(e => e.Cnpj)
                .IsRequired()
                .HasMaxLength(14);

            empresa.Property(e => e.Endereco)
                .IsRequired()
                .HasMaxLength(400);

            empresa.Property(e => e.Telefone)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}