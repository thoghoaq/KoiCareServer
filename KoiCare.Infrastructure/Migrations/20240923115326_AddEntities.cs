using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KoiCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogPost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPost_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KoiType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoiType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Total = table.Column<decimal>(type: "numeric", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Users_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pond",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Length = table.Column<decimal>(type: "numeric", nullable: false),
                    Width = table.Column<decimal>(type: "numeric", nullable: false),
                    Depth = table.Column<decimal>(type: "numeric", nullable: false),
                    Volume = table.Column<decimal>(type: "numeric", nullable: false),
                    DrainageCount = table.Column<decimal>(type: "numeric", nullable: false),
                    PumpCapacity = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pond", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pond_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerfectWaterVolume",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KoiTypeId = table.Column<int>(type: "integer", nullable: false),
                    MinVolume = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxVolume = table.Column<decimal>(type: "numeric", nullable: false),
                    OptimalVolume = table.Column<decimal>(type: "numeric", nullable: false),
                    WaterLevel = table.Column<decimal>(type: "numeric", nullable: false),
                    RecommendedSaltLevel = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfectWaterVolume", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfectWaterVolume_KoiType_KoiTypeId",
                        column: x => x.KoiTypeId,
                        principalTable: "KoiType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KoiIndividual",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KoiTypeId = table.Column<int>(type: "integer", nullable: false),
                    PondId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Age = table.Column<decimal>(type: "numeric", nullable: true),
                    Length = table.Column<decimal>(type: "numeric", nullable: true),
                    Weight = table.Column<decimal>(type: "numeric", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    Origin = table.Column<int>(type: "integer", nullable: true),
                    Shape = table.Column<int>(type: "integer", nullable: true),
                    Breed = table.Column<decimal>(type: "numeric", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoiIndividual", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KoiIndividual_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KoiIndividual_KoiType_KoiTypeId",
                        column: x => x.KoiTypeId,
                        principalTable: "KoiType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KoiIndividual_Pond_PondId",
                        column: x => x.PondId,
                        principalTable: "Pond",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaltRequirement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PondId = table.Column<int>(type: "integer", nullable: false),
                    RequiredAmount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaltRequirement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaltRequirement_Pond_PondId",
                        column: x => x.PondId,
                        principalTable: "Pond",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WaterParameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PondId = table.Column<int>(type: "integer", nullable: false),
                    Temperature = table.Column<decimal>(type: "numeric", nullable: true),
                    Salinity = table.Column<decimal>(type: "numeric", nullable: true),
                    Ph = table.Column<decimal>(type: "numeric", nullable: true),
                    Oxygen = table.Column<decimal>(type: "numeric", nullable: true),
                    NO2 = table.Column<decimal>(type: "numeric", nullable: true),
                    NO3 = table.Column<decimal>(type: "numeric", nullable: true),
                    PO4 = table.Column<decimal>(type: "numeric", nullable: true),
                    MeasuredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterParameter_Pond_PondId",
                        column: x => x.PondId,
                        principalTable: "Pond",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedCalculation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KoiIndividualId = table.Column<int>(type: "integer", nullable: false),
                    DailyAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Frequency = table.Column<decimal>(type: "numeric", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CalculationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedCalculation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedCalculation_KoiIndividual_KoiIndividualId",
                        column: x => x.KoiIndividualId,
                        principalTable: "KoiIndividual",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedingSchedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KoiIndividualId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Period = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedingSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedingSchedule_KoiIndividual_KoiIndividualId",
                        column: x => x.KoiIndividualId,
                        principalTable: "KoiIndividual",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KoiGrowth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KoiIndividualId = table.Column<int>(type: "integer", nullable: false),
                    Length = table.Column<decimal>(type: "numeric", nullable: false),
                    Weight = table.Column<decimal>(type: "numeric", nullable: false),
                    MeasuredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoiGrowth", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KoiGrowth_KoiIndividual_KoiIndividualId",
                        column: x => x.KoiIndividualId,
                        principalTable: "KoiIndividual",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPost_AuthorId",
                table: "BlogPost",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedCalculation_KoiIndividualId",
                table: "FeedCalculation",
                column: "KoiIndividualId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedingSchedule_KoiIndividualId",
                table: "FeedingSchedule",
                column: "KoiIndividualId");

            migrationBuilder.CreateIndex(
                name: "IX_KoiGrowth_KoiIndividualId",
                table: "KoiGrowth",
                column: "KoiIndividualId");

            migrationBuilder.CreateIndex(
                name: "IX_KoiIndividual_CategoryId",
                table: "KoiIndividual",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_KoiIndividual_KoiTypeId",
                table: "KoiIndividual",
                column: "KoiTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_KoiIndividual_PondId",
                table: "KoiIndividual",
                column: "PondId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId",
                table: "Order",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProductId",
                table: "OrderDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfectWaterVolume_KoiTypeId",
                table: "PerfectWaterVolume",
                column: "KoiTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Pond_OwnerId",
                table: "Pond",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SaltRequirement_PondId",
                table: "SaltRequirement",
                column: "PondId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WaterParameter_PondId",
                table: "WaterParameter",
                column: "PondId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPost");

            migrationBuilder.DropTable(
                name: "FeedCalculation");

            migrationBuilder.DropTable(
                name: "FeedingSchedule");

            migrationBuilder.DropTable(
                name: "KoiGrowth");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "PerfectWaterVolume");

            migrationBuilder.DropTable(
                name: "SaltRequirement");

            migrationBuilder.DropTable(
                name: "WaterParameter");

            migrationBuilder.DropTable(
                name: "KoiIndividual");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "KoiType");

            migrationBuilder.DropTable(
                name: "Pond");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
