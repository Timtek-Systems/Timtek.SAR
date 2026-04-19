using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Timtek.SAR.Domain.Entities;

namespace Timtek.SAR.Infrastructure.Persistence.Configurations;

public sealed class CaseConfiguration : IEntityTypeConfiguration<Case>
{
    public void Configure(EntityTypeBuilder<Case> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.ReferenceNumber).IsRequired().HasMaxLength(30);
        builder.HasIndex(c => c.ReferenceNumber).IsUnique();
        builder.Property(c => c.OrganisationId).IsRequired();
        builder.Property(c => c.CreatedByUserId).IsRequired();
        builder.Property(c => c.CreatedAtUtc).IsRequired();

        builder.Property(c => c.AnimalSpecies).IsRequired().HasMaxLength(100);
        builder.Property(c => c.AnimalBreed).HasMaxLength(100);
        builder.Property(c => c.AnimalColour).HasMaxLength(100);
        builder.Property(c => c.AnimalSize).HasMaxLength(50);
        builder.Property(c => c.AnimalDistinguishingFeatures).HasMaxLength(1000);
        builder.Property(c => c.AnimalPhotoUrl).HasMaxLength(2048);

        builder.Property(c => c.OwnerName).IsRequired().HasMaxLength(200);
        builder.Property(c => c.OwnerPhone).HasMaxLength(30);
        builder.Property(c => c.OwnerEmail).IsRequired().HasMaxLength(254);
        builder.Property(c => c.MedicalNotes).HasMaxLength(4000);
        builder.Property(c => c.BehaviouralNotes).HasMaxLength(4000);

        builder.Property(c => c.Status).IsRequired().HasConversion<string>().HasMaxLength(20);
        builder.Property(c => c.Priority).HasConversion<string>().HasMaxLength(20);
        builder.Property(c => c.Outcome).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(c => c.OrganisationId);
        builder.HasIndex(c => c.Status);

        builder.HasMany(c => c.ActivityLog)
            .WithOne()
            .HasForeignKey(a => a.CaseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class CaseActivityLogConfiguration : IEntityTypeConfiguration<CaseActivityLog>
{
    public void Configure(EntityTypeBuilder<CaseActivityLog> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.CaseId).IsRequired();
        builder.Property(a => a.UserId).IsRequired();
        builder.Property(a => a.ActivityType).IsRequired().HasConversion<string>().HasMaxLength(30);
        builder.Property(a => a.Description).IsRequired().HasMaxLength(4000);
        builder.Property(a => a.TimestampUtc).IsRequired();

        builder.HasIndex(a => a.CaseId);
    }
}
