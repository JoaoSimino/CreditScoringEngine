
using CreditScoringEngine.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditScoringEngine.Infrastructure.Data;

public class CreditScoringEngineContext : DbContext
{
    public CreditScoringEngineContext(DbContextOptions<CreditScoringEngineContext> options) : base(options)
    {}

    public DbSet<Cliente> Clientes { get; set; } = default!;
}
