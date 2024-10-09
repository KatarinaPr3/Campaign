using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampaignAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddingCampaignMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Campaign",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    CampaignType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaign", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Campaign",
                columns: new[] { "Id", "CampaignName", "CampaignType", "Discount", "EndDate", "StartDate" },
                values: new object[] { 1, "Big Discount Campaign", 0, 20.0, new DateTime(2024, 12, 31, 23, 59, 59, 0, DateTimeKind.Unspecified), new DateTime(2024, 10, 9, 0, 0, 0, 0, DateTimeKind.Local) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Campaign");
        }
    }
}
