using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNSVM.Migrations
{
    /// <inheritdoc />
    public partial class ModifyMedicamentPrescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicamentPrescription_User_UserId",
                table: "MedicamentPrescription");

            migrationBuilder.DropIndex(
                name: "IX_MedicamentPrescription_UserId",
                table: "MedicamentPrescription");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MedicamentPrescription");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "MedicamentPrescription",
                newName: "IdDoctor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdDoctor",
                table: "MedicamentPrescription",
                newName: "IdUser");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "MedicamentPrescription",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentPrescription_UserId",
                table: "MedicamentPrescription",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicamentPrescription_User_UserId",
                table: "MedicamentPrescription",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
