using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace fonenako_service.Migrations
{
    public partial class initDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Localisation",
                columns: table => new
                {
                    LocalisationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    HierarchyId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localisation", x => x.LocalisationId);
                    table.ForeignKey(
                        name: "FK_Localisation_Localisation_HierarchyId",
                        column: x => x.HierarchyId,
                        principalTable: "Localisation",
                        principalColumn: "LocalisationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaseOffer",
                columns: table => new
                {
                    OfferId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Surface = table.Column<double>(type: "double precision", nullable: false),
                    Rooms = table.Column<int>(type: "integer", nullable: false),
                    MonthlyRent = table.Column<double>(type: "double precision", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Carousel = table.Column<string>(type: "text", nullable: true),
                    Photos = table.Column<string>(type: "text", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LocalisationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaseOffer", x => x.OfferId);
                    table.ForeignKey(
                        name: "FK_LeaseOffer_Localisation_LocalisationId",
                        column: x => x.LocalisationId,
                        principalTable: "Localisation",
                        principalColumn: "LocalisationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaseOffer_LocalisationId",
                table: "LeaseOffer",
                column: "LocalisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Localisation_HierarchyId",
                table: "Localisation",
                column: "HierarchyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaseOffer");

            migrationBuilder.DropTable(
                name: "Localisation");
        }
    }
}
