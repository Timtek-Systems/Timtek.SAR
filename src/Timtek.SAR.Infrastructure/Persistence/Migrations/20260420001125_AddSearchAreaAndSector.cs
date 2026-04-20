using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Timtek.SAR.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSearchAreaAndSector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CaseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    AreaType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    GeoJson = table.Column<string>(type: "TEXT", nullable: false),
                    RadiusMetres = table.Column<double>(type: "REAL", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sectors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SearchAreaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    GeoJson = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    AssignedToUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sectors_SearchAreas_SearchAreaId",
                        column: x => x.SearchAreaId,
                        principalTable: "SearchAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SearchAreas_CaseId",
                table: "SearchAreas",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Sectors_SearchAreaId",
                table: "Sectors",
                column: "SearchAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sectors_Status",
                table: "Sectors",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sectors");

            migrationBuilder.DropTable(
                name: "SearchAreas");
        }
    }
}
