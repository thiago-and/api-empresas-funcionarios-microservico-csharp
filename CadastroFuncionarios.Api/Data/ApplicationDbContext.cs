using CadastroFuncionarios.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroFuncionarios.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Funcionario> Funcionarios => Set<Funcionario>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var funcionario = modelBuilder.Entity<Funcionario>();

            funcionario.ToTable("Funcionarios");
            funcionario.HasKey(f => f.Id);
            funcionario.Property(f => f.Nome)
                .IsRequired()
                .HasMaxLength(150);

            funcionario.Property(f => f.Cargo)
                .IsRequired()
                .HasMaxLength(100);

            funcionario.Property(f => f.Salario)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            funcionario.Property(f => f.DataAdmissao)
                .IsRequired();
        }
    }
}