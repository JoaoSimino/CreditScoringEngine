using Microsoft.EntityFrameworkCore;
using MinimalAPIKickoff.Domain.Entities;

namespace MinimalAPIKickoff.Infrastructure.Data;

public class MinimalAPIKickoffContext : DbContext
{
    public MinimalAPIKickoffContext(DbContextOptions<MinimalAPIKickoffContext> options) : base(options)
    {}

    public DbSet<User> Users { get; set; } = default!;
}
