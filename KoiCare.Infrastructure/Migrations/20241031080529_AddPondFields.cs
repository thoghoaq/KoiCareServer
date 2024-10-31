using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoiCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPondFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgeRange",
                table: "Ponds",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Ponds",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KoiGroupId",
                table: "Ponds",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ponds_KoiGroupId",
                table: "Ponds",
                column: "KoiGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ponds_KoiGroups_KoiGroupId",
                table: "Ponds",
                column: "KoiGroupId",
                principalTable: "KoiGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ponds_KoiGroups_KoiGroupId",
                table: "Ponds");

            migrationBuilder.DropIndex(
                name: "IX_Ponds_KoiGroupId",
                table: "Ponds");

            migrationBuilder.DropColumn(
                name: "AgeRange",
                table: "Ponds");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Ponds");

            migrationBuilder.DropColumn(
                name: "KoiGroupId",
                table: "Ponds");
        }
    }
}
