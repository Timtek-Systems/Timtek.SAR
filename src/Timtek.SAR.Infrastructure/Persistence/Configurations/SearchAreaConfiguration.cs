using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Timtek.SAR.Domain.Entities;

namespace Timtek.SAR.Infrastructure.Persistence.Configurations;

public sealed class SearchAreaConfiguration : IEntityTypeConfiguration<SearchArea>
{
    public void Configure(EntityTypeBuilder<SearchArea> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.CaseId).IsRequired();
        builder.Property(a => a.Name).IsRequired().HasMaxLength(200);
        builder.Property(a => a.AreaType).IsRequired().HasConversion<string>().HasMaxLength(20);
        builder.Property(a => a.GeoJson).IsRequired();
        builder.Property(a => a.RadiusMetres);
        builder.Property(a => a.CreatedAtUtc).IsRequired();

        builder.HasIndex(a => a.CaseId);

        builder.HasMany(a => a.Sectors)
            .WithOne()
            .HasForeignKey(s => s.SearchAreaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class SectorConfiguration : IEntityTypeConfiguration<Sector>
{
    public void Configure(EntityTypeBuilder<Sector> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.SearchAreaId).IsRequired();
        builder.Property(s => s.Name).IsRequired().HasMaxLength(200);
        builder.Property(s => s.GeoJson).IsRequired();
        builder.Property(s => s.Status).IsRequired().HasConversion<string>().HasMaxLength(20);
        builder.Property(s => s.AssignedToUserId);
        builder.Property(s => s.CreatedAtUtc).IsRequired();

        builder.HasIndex(s => s.SearchAreaId);
        builder.HasIndex(s => s.Status);
    }
}
