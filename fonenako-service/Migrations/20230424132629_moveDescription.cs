using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace fonenako_service.Migrations
{
    public partial class moveDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeaseOfferDescription",
                columns: table => new
                {
                    LeaseOfferDescriptionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    LeaseOfferId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaseOfferDescription", x => x.LeaseOfferDescriptionId);
                    table.ForeignKey(
                        name: "FK_LeaseOfferDescription_LeaseOffer_LeaseOfferId",
                        column: x => x.LeaseOfferId,
                        principalTable: "LeaseOffer",
                        principalColumn: "OfferId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeaseOfferDescription_LeaseOfferId",
                table: "LeaseOfferDescription",
                column: "LeaseOfferId",
                unique: true);

            migrationBuilder.Sql("insert into \"LeaseOfferDescription\"(\"Content\", \"LeaseOfferId\") select \"Description\",\"OfferId\" from \"LeaseOffer\"");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LeaseOffer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaseOfferDescription");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LeaseOffer",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
