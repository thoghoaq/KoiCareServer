using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KoiCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyPond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedingSchedules_KoiIndividuals_KoiIndividualId",
                table: "FeedingSchedules");

            migrationBuilder.RenameColumn(
                name: "KoiIndividualId",
                table: "FeedingSchedules",
                newName: "PondId");

            migrationBuilder.RenameIndex(
                name: "IX_FeedingSchedules_KoiIndividualId",
                table: "FeedingSchedules",
                newName: "IX_FeedingSchedules_PondId");

            migrationBuilder.AddColumn<int>(
                name: "KoiGroupId",
                table: "KoiTypes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "KoiGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoiGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServingSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AgeRange = table.Column<int>(type: "integer", nullable: false),
                    WeightPercent = table.Column<decimal>(type: "numeric", nullable: false),
                    FoodDescription = table.Column<string>(type: "text", nullable: false),
                    DailyFrequency = table.Column<string>(type: "text", nullable: false),
                    KoiGroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServingSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServingSizes_KoiGroups_KoiGroupId",
                        column: x => x.KoiGroupId,
                        principalTable: "KoiGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "KoiGroups",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Nhóm 1" },
                    { 2, "Nhóm 2" },
                    { 3, "Nhóm 3" },
                    { 4, "Nhóm 4" },
                    { 5, "Nhóm 5" }
                });

            migrationBuilder.InsertData(
                table: "KoiTypes",
                columns: new[] { "Id", "Description", "KoiGroupId", "Name" },
                values: new object[,]
                {
                    { 1, "Kohaku là loại koi có nền trắng với các mảng đỏ (hi thường gọi là “hi”) trên thân. Đây là loại koi cơ bản và được xem là quan trọng nhất trong các dòng cá koi.", 1, "Kohaku" },
                    { 2, "Sanke có nền trắng với các mảng đỏ và đen trên thân. Mảng đen thường tập trung ở phần thân sau và không xuất hiện trên đầu.", 1, "Taisho Sanke (Sanke)" },
                    { 3, "Showa có nền đen với các mảng đỏ và trắng trên thân. Khác với Sanke, Showa có mảng đen (sumi) xuất hiện trên đầu.", 1, "Showa Sanshoku (Showa)" },
                    { 4, "Utsuri có nền đen với các mảng màu khác như trắng, đỏ, hoặc vàng. Tùy thuộc vào màu phụ, Utsuri được phân loại thành Shiro Utsuri (đen và trắng), Hi Utsuri (đen và đỏ), hoặc Ki Utsuri (đen và vàng).", 1, "Utsurimono (Utsuri)" },
                    { 5, "Bekko có nền trắng, đỏ, hoặc vàng với các mảng đen (sumi) trên thân. Loại koi này có ba biến thể chính là Shiro Bekko (trắng và đen), Aka Bekko (đỏ và đen), và Ki Bekko (vàng và đen).", 1, "Bekko" },
                    { 6, "Asagi có thân màu xanh lơ với các vảy được xếp thành hàng theo kiểu lưới, bụng và các vây thường có màu đỏ hoặc cam.", 2, "Asagi" },
                    { 7, "Tancho có nền trắng tinh khiết với một đốm đỏ tròn trên đầu, tượng trưng cho lá cờ của Nhật Bản. Đây là một loại cá rất quý hiếm và được yêu thích.", 3, "Tancho" },
                    { 8, "Goshiki có màu nền xanh xám với các vảy màu đen, trắng, đỏ, và xanh. Màu đỏ thường chiếm ưu thế ở phần thân.", 5, "Goshiki" },
                    { 9, "Kawarimono là một nhóm gồm những loại koi không thuộc các dòng cơ bản khác. Chúng có nhiều hình dạng và màu sắc khác nhau, từ xanh lá, đen, đến bạc.", 4, "Kawarimono" },
                    { 10, "Ogons là dòng koi kim loại với màu sắc sáng bóng, thường có các màu như vàng, bạch kim, hoặc đồng.", 3, "Ogons" },
                    { 11, "Doitsu Koi là một dòng koi không có vảy hoặc chỉ có vảy dọc theo phần lưng. Loại này có thể xuất hiện trong nhiều biến thể như Kohaku, Sanke, và Showa.", 4, "Doitsu Koi" },
                    { 12, "Shusui là một phiên bản không có vảy của Asagi, với thân màu xanh và các mảng đỏ hoặc cam chạy dọc theo thân và bụng.", 2, "Shusui" }
                });

            migrationBuilder.InsertData(
                table: "ServingSizes",
                columns: new[] { "Id", "AgeRange", "DailyFrequency", "FoodDescription", "KoiGroupId", "WeightPercent" },
                values: new object[,]
                {
                    { 1, 1, "3", "Hạt nhỏ giàu protein (35-40%) / vitamin tăng cường màu sắc", 1, 0.04m },
                    { 2, 2, "2", "Hạt protein cao (30-35%), bổ sung vitamin và khoáng chất", 1, 0.015m },
                    { 3, 3, "1-2", "Hạt lớn, protein 30%, nhiều chất xơ để hỗ trợ tiêu hóa", 1, 0.01m },
                    { 4, 1, "3", "Hạt protein cao (35-40%), nhiều carotenoid cho màu sắc", 2, 0.04m },
                    { 5, 2, "2", "Protein 30-35%, bổ sung thêm canxi và chất xơ", 2, 0.015m },
                    { 6, 3, "1-2", "Hạt lớn, bổ sung vitamin tăng cường", 2, 0.01m },
                    { 7, 1, "3", "Protein cao (35%), bổ sung vitamin C và D", 3, 0.04m },
                    { 8, 2, "2", "Protein 30%, bổ sung vi lượng tăng cường độ bóng", 3, 0.015m },
                    { 9, 3, "1", "Hạt lớn, protein thấp hơn nhưng nhiều xơ", 3, 0.01m },
                    { 10, 1, "3", "Hạt giàu protein (35-40%), dễ tiêu hóa", 4, 0.04m },
                    { 11, 2, "2", "Protein 30-35%, bổ sung chất béo cho sự phát triển", 4, 0.015m },
                    { 12, 3, "1", "Hạt tổng hợp, cân bằng dinh dưỡng", 4, 0.01m },
                    { 13, 1, "3", "Hạt giàu protein (35-40%), nhiều carotenoid", 5, 0.04m },
                    { 14, 2, "2", "Protein 30%, vitamin E và A", 5, 0.015m },
                    { 15, 3, "1", "Hạt bổ sung chất xơ và chất chống oxy hóa", 5, 0.01m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_KoiTypes_KoiGroupId",
                table: "KoiTypes",
                column: "KoiGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ServingSizes_KoiGroupId",
                table: "ServingSizes",
                column: "KoiGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedingSchedules_Ponds_PondId",
                table: "FeedingSchedules",
                column: "PondId",
                principalTable: "Ponds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KoiTypes_KoiGroups_KoiGroupId",
                table: "KoiTypes",
                column: "KoiGroupId",
                principalTable: "KoiGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedingSchedules_Ponds_PondId",
                table: "FeedingSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_KoiTypes_KoiGroups_KoiGroupId",
                table: "KoiTypes");

            migrationBuilder.DropTable(
                name: "ServingSizes");

            migrationBuilder.DropTable(
                name: "KoiGroups");

            migrationBuilder.DropIndex(
                name: "IX_KoiTypes_KoiGroupId",
                table: "KoiTypes");

            migrationBuilder.DeleteData(
                table: "KoiTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "KoiTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "KoiTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "KoiTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "KoiTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "KoiTypes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "KoiTypes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "KoiTypes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "KoiTypes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "KoiTypes",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "KoiTypes",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "KoiTypes",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DropColumn(
                name: "KoiGroupId",
                table: "KoiTypes");

            migrationBuilder.RenameColumn(
                name: "PondId",
                table: "FeedingSchedules",
                newName: "KoiIndividualId");

            migrationBuilder.RenameIndex(
                name: "IX_FeedingSchedules_PondId",
                table: "FeedingSchedules",
                newName: "IX_FeedingSchedules_KoiIndividualId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedingSchedules_KoiIndividuals_KoiIndividualId",
                table: "FeedingSchedules",
                column: "KoiIndividualId",
                principalTable: "KoiIndividuals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
