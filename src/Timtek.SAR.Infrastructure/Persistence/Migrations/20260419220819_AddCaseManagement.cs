using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Timtek.SAR.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCaseManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrganisationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AnimalSpecies = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    AnimalBreed = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    AnimalColour = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    AnimalSize = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    AnimalDistinguishingFeatures = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    AnimalPhotoUrl = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    LastKnownLatitude = table.Column<double>(type: "REAL", nullable: false),
                    LastKnownLongitude = table.Column<double>(type: "REAL", nullable: false),
                    DateTimeLastSeen = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OwnerName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    OwnerPhone = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    OwnerEmail = table.Column<string>(type: "TEXT", maxLength: 254, nullable: false),
                    MedicalNotes = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    BehaviouralNotes = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Priority = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Outcome = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaseActivityLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CaseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ActivityType = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                    TimestampUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseActivityLogs_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseActivityLogs_CaseId",
                table: "CaseActivityLogs",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_OrganisationId",
                table: "Cases",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ReferenceNumber",
                table: "Cases",
                column: "ReferenceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cases_Status",
                table: "Cases",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseActivityLogs");

            migrationBuilder.DropTable(
                name: "Cases");
        }
    }
}
