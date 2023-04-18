using Microsoft.EntityFrameworkCore.Migrations;

namespace fonenako_service.Migrations
{
    public partial class RemoveUselessExcplicitForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaseOffer_Localisation_LocalisationId",
                table: "LeaseOffer");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaseOffer_Localisation_LocalisationId1",
                table: "LeaseOffer");

            migrationBuilder.DropForeignKey(
                name: "FK_Localisation_Localisation_HierarchyId",
                table: "Localisation");

            migrationBuilder.RenameColumn(
                name: "HierarchyId",
                table: "Localisation",
                newName: "HierarchyLocalisationId");

            migrationBuilder.RenameIndex(
                name: "IX_Localisation_HierarchyId",
                table: "Localisation",
                newName: "IX_Localisation_HierarchyLocalisationId");

            migrationBuilder.AlterColumn<int>(
                name: "LocalisationId1",
                table: "LeaseOffer",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LocalisationId",
                table: "LeaseOffer",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaseOffer_Localisation_LocalisationId",
                table: "LeaseOffer",
                column: "LocalisationId",
                principalTable: "Localisation",
                principalColumn: "LocalisationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaseOffer_Localisation_LocalisationId1",
                table: "LeaseOffer",
                column: "LocalisationId1",
                principalTable: "Localisation",
                principalColumn: "LocalisationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Localisation_Localisation_HierarchyLocalisationId",
                table: "Localisation",
                column: "HierarchyLocalisationId",
                principalTable: "Localisation",
                principalColumn: "LocalisationId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaseOffer_Localisation_LocalisationId",
                table: "LeaseOffer");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaseOffer_Localisation_LocalisationId1",
                table: "LeaseOffer");

            migrationBuilder.DropForeignKey(
                name: "FK_Localisation_Localisation_HierarchyLocalisationId",
                table: "Localisation");

            migrationBuilder.RenameColumn(
                name: "HierarchyLocalisationId",
                table: "Localisation",
                newName: "HierarchyId");

            migrationBuilder.RenameIndex(
                name: "IX_Localisation_HierarchyLocalisationId",
                table: "Localisation",
                newName: "IX_Localisation_HierarchyId");

            migrationBuilder.AlterColumn<int>(
                name: "LocalisationId1",
                table: "LeaseOffer",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "LocalisationId",
                table: "LeaseOffer",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaseOffer_Localisation_LocalisationId",
                table: "LeaseOffer",
                column: "LocalisationId",
                principalTable: "Localisation",
                principalColumn: "LocalisationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaseOffer_Localisation_LocalisationId1",
                table: "LeaseOffer",
                column: "LocalisationId1",
                principalTable: "Localisation",
                principalColumn: "LocalisationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Localisation_Localisation_HierarchyId",
                table: "Localisation",
                column: "HierarchyId",
                principalTable: "Localisation",
                principalColumn: "LocalisationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
