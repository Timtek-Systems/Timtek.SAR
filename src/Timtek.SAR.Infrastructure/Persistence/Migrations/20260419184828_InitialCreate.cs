using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Timtek.SAR.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organisations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    DefaultAoRadiusKm = table.Column<double>(type: "REAL", nullable: false),
                    DefaultReputationScore = table.Column<int>(type: "INTEGER", nullable: false),
                    MinimumReputationThreshold = table.Column<int>(type: "INTEGER", nullable: false),
                    ExtendedNotificationRadiusKm = table.Column<double>(type: "REAL", nullable: false),
                    KeeperInvitationExpiryDays = table.Column<int>(type: "INTEGER", nullable: false),
                    KeeperRetentionDays = table.Column<int>(type: "INTEGER", nullable: false),
                    ReputationPts_JoinCase = table.Column<int>(type: "INTEGER", nullable: false),
                    ReputationPts_FoundOutcome = table.Column<int>(type: "INTEGER", nullable: false),
                    ReputationPts_ConfirmedSighting = table.Column<int>(type: "INTEGER", nullable: false),
                    ReputationPts_EvidenceUpload = table.Column<int>(type: "INTEGER", nullable: false),
                    ReputationPts_EvidenceUploadCapPerCase = table.Column<int>(type: "INTEGER", nullable: false),
                    ReputationPts_TrackUpload = table.Column<int>(type: "INTEGER", nullable: false),
                    ReputationPts_SectorCompletion = table.Column<int>(type: "INTEGER", nullable: false),
                    ReputationPts_FirstCaseBonus = table.Column<int>(type: "INTEGER", nullable: false),
                    ReputationPts_MissedInScopeCase = table.Column<int>(type: "INTEGER", nullable: false),
                    ReputationPts_RemovedFromCase = table.Column<int>(type: "INTEGER", nullable: false),
                    ReputationPts_DismissedSighting = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Organisations");
        }
    }
}
