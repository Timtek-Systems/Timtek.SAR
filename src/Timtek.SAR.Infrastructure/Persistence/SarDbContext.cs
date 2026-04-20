using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Timtek.SAR.Domain.Entities;

namespace Timtek.SAR.Infrastructure.Persistence;

public sealed class SarDbContext(DbContextOptions<SarDbContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    public DbSet<Organisation> Organisations => Set<Organisation>();
    public DbSet<Case> Cases => Set<Case>();
    public DbSet<CaseActivityLog> CaseActivityLogs => Set<CaseActivityLog>();
    public DbSet<SearchArea> SearchAreas => Set<SearchArea>();
    public DbSet<Sector> Sectors => Set<Sector>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SarDbContext).Assembly);
    }
}
