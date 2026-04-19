using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Timtek.SAR.Domain.Entities;

namespace Timtek.SAR.Infrastructure.Persistence.Configurations;

public sealed class OrganisationConfiguration : IEntityTypeConfiguration<Organisation>
{
    public void Configure(EntityTypeBuilder<Organisation> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Name).IsRequired().HasMaxLength(200);

        builder.OwnsOne(o => o.ReputationPointValues, rpv =>
        {
            rpv.Property(r => r.JoinCase).HasColumnName("ReputationPts_JoinCase");
            rpv.Property(r => r.FoundOutcome).HasColumnName("ReputationPts_FoundOutcome");
            rpv.Property(r => r.ConfirmedSighting).HasColumnName("ReputationPts_ConfirmedSighting");
            rpv.Property(r => r.EvidenceUpload).HasColumnName("ReputationPts_EvidenceUpload");
            rpv.Property(r => r.EvidenceUploadCapPerCase).HasColumnName("ReputationPts_EvidenceUploadCapPerCase");
            rpv.Property(r => r.TrackUpload).HasColumnName("ReputationPts_TrackUpload");
            rpv.Property(r => r.SectorCompletion).HasColumnName("ReputationPts_SectorCompletion");
            rpv.Property(r => r.FirstCaseBonus).HasColumnName("ReputationPts_FirstCaseBonus");
            rpv.Property(r => r.MissedInScopeCase).HasColumnName("ReputationPts_MissedInScopeCase");
            rpv.Property(r => r.RemovedFromCase).HasColumnName("ReputationPts_RemovedFromCase");
            rpv.Property(r => r.DismissedSighting).HasColumnName("ReputationPts_DismissedSighting");
        });
    }
}
