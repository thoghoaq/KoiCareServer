using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoiCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCategoryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KoiIndividuals_Categories_CategoryId",
                table: "KoiIndividuals");

            migrationBuilder.DropIndex(
                name: "IX_KoiIndividuals_CategoryId",
                table: "KoiIndividuals");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "KoiIndividuals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "KoiIndividuals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_KoiIndividuals_CategoryId",
                table: "KoiIndividuals",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_KoiIndividuals_Categories_CategoryId",
                table: "KoiIndividuals",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
