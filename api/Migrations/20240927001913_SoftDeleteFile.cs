using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace auth.Migrations
{
    /// <inheritdoc />
    public partial class SoftDeleteFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_SignatureProcesses_SignatureProcessId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "SignatureProcesses");

            migrationBuilder.RenameColumn(
                name: "SignatureProcessId",
                table: "Files",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Files_SignatureProcessId",
                table: "Files",
                newName: "IX_Files_UserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Files",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Users_UserId",
                table: "Files",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Users_UserId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Files",
                newName: "SignatureProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_Files_UserId",
                table: "Files",
                newName: "IX_Files_SignatureProcessId");

            migrationBuilder.CreateTable(
                name: "SignatureProcesses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignatureProcesses", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Files_SignatureProcesses_SignatureProcessId",
                table: "Files",
                column: "SignatureProcessId",
                principalTable: "SignatureProcesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
