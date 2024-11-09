using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNSVM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicamentPrescription_Prescription_PrescriptionId",
                table: "MedicamentPrescription");

            migrationBuilder.DropTable(
                name: "DoctorGroup");

            migrationBuilder.DropTable(
                name: "MedicalGroupAudit");

            migrationBuilder.DropTable(
                name: "Prescription");

            migrationBuilder.DropTable(
                name: "MedicalGroup");

            migrationBuilder.DropIndex(
                name: "IX_MedicamentPrescription_PrescriptionId",
                table: "MedicamentPrescription");

            migrationBuilder.RenameColumn(
                name: "PrescriptionId",
                table: "MedicamentPrescription",
                newName: "id_historia");

            migrationBuilder.AlterColumn<string>(
                name: "Specialty",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "IdUser",
                table: "MedicamentPrescription",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PrescriptionIdHistoria",
                table: "MedicamentPrescription",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "MedicamentPrescription",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClinicalHistoryIdHistoria",
                table: "Medicament",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClinicalHistory",
                columns: table => new
                {
                    IdHistoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Diagnostico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Antecedentes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicalHistory", x => x.IdHistoria);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentPrescription_PrescriptionIdHistoria",
                table: "MedicamentPrescription",
                column: "PrescriptionIdHistoria");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentPrescription_UserId",
                table: "MedicamentPrescription",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicament_ClinicalHistoryIdHistoria",
                table: "Medicament",
                column: "ClinicalHistoryIdHistoria");

            migrationBuilder.AddForeignKey(
                name: "FK_Medicament_ClinicalHistory_ClinicalHistoryIdHistoria",
                table: "Medicament",
                column: "ClinicalHistoryIdHistoria",
                principalTable: "ClinicalHistory",
                principalColumn: "IdHistoria");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicamentPrescription_ClinicalHistory_PrescriptionIdHistoria",
                table: "MedicamentPrescription",
                column: "PrescriptionIdHistoria",
                principalTable: "ClinicalHistory",
                principalColumn: "IdHistoria");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicamentPrescription_User_UserId",
                table: "MedicamentPrescription",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medicament_ClinicalHistory_ClinicalHistoryIdHistoria",
                table: "Medicament");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicamentPrescription_ClinicalHistory_PrescriptionIdHistoria",
                table: "MedicamentPrescription");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicamentPrescription_User_UserId",
                table: "MedicamentPrescription");

            migrationBuilder.DropTable(
                name: "ClinicalHistory");

            migrationBuilder.DropIndex(
                name: "IX_MedicamentPrescription_PrescriptionIdHistoria",
                table: "MedicamentPrescription");

            migrationBuilder.DropIndex(
                name: "IX_MedicamentPrescription_UserId",
                table: "MedicamentPrescription");

            migrationBuilder.DropIndex(
                name: "IX_Medicament_ClinicalHistoryIdHistoria",
                table: "Medicament");

            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "MedicamentPrescription");

            migrationBuilder.DropColumn(
                name: "PrescriptionIdHistoria",
                table: "MedicamentPrescription");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MedicamentPrescription");

            migrationBuilder.DropColumn(
                name: "ClinicalHistoryIdHistoria",
                table: "Medicament");

            migrationBuilder.RenameColumn(
                name: "id_historia",
                table: "MedicamentPrescription",
                newName: "PrescriptionId");

            migrationBuilder.AlterColumn<string>(
                name: "Specialty",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MedicalGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prescription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prescription_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prescription_User_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorGroup",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorGroup", x => new { x.UserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_DoctorGroup_MedicalGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "MedicalGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorGroup_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicalGroupAudit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalGroupAudit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalGroupAudit_MedicalGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "MedicalGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalGroupAudit_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentPrescription_PrescriptionId",
                table: "MedicamentPrescription",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorGroup_GroupId",
                table: "DoctorGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalGroupAudit_GroupId",
                table: "MedicalGroupAudit",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalGroupAudit_UserId",
                table: "MedicalGroupAudit",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_DoctorId",
                table: "Prescription",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_PatientId",
                table: "Prescription",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicamentPrescription_Prescription_PrescriptionId",
                table: "MedicamentPrescription",
                column: "PrescriptionId",
                principalTable: "Prescription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
