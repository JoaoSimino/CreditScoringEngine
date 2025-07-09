
using CreditScoringEngine.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditScoringEngine.Infrastructure.Data;

public class CreditScoringEngineContext : DbContext
{
    public CreditScoringEngineContext(DbContextOptions<CreditScoringEngineContext> options) : base(options)
    {
    }

    public DbSet<PropostaCredito> PropostasCredito { get; set; }
    public DbSet<Cliente> Clientes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PropostaCredito>()
            .HasOne(pc => pc.Cliente)
            .WithOne(c => c.PropostaCredito)
            .HasForeignKey<PropostaCredito>(pc => pc.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PropostaCredito>()
            .Property(pc => pc.Status)
            .HasConversion<string>(); //sendo enum para eu salvar o texto em vez de numeros!

        modelBuilder.Entity<PropostaCredito>()// Objeto de Valor sem tabela propria
            .OwnsOne(pc => pc.Score,sa => 
            {
                sa.Property(s => s.Valor).HasColumnName("ScoreValor");
                
                sa.Property(s => s.Faixa).HasColumnName("ScoreFaixa").HasConversion<string>();

            });


        base.OnModelCreating(modelBuilder);
    }
}
