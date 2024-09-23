using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoiCare.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDbSetEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPost_Users_AuthorId",
                table: "BlogPost");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedCalculation_KoiIndividual_KoiIndividualId",
                table: "FeedCalculation");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedingSchedule_KoiIndividual_KoiIndividualId",
                table: "FeedingSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_KoiGrowth_KoiIndividual_KoiIndividualId",
                table: "KoiGrowth");

            migrationBuilder.DropForeignKey(
                name: "FK_KoiIndividual_Category_CategoryId",
                table: "KoiIndividual");

            migrationBuilder.DropForeignKey(
                name: "FK_KoiIndividual_KoiType_KoiTypeId",
                table: "KoiIndividual");

            migrationBuilder.DropForeignKey(
                name: "FK_KoiIndividual_Pond_PondId",
                table: "KoiIndividual");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Users_CustomerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Order_OrderId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Product_ProductId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_PerfectWaterVolume_KoiType_KoiTypeId",
                table: "PerfectWaterVolume");

            migrationBuilder.DropForeignKey(
                name: "FK_Pond_Users_OwnerId",
                table: "Pond");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_SaltRequirement_Pond_PondId",
                table: "SaltRequirement");

            migrationBuilder.DropForeignKey(
                name: "FK_WaterParameter_Pond_PondId",
                table: "WaterParameter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WaterParameter",
                table: "WaterParameter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaltRequirement",
                table: "SaltRequirement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pond",
                table: "Pond");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PerfectWaterVolume",
                table: "PerfectWaterVolume");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KoiType",
                table: "KoiType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KoiIndividual",
                table: "KoiIndividual");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KoiGrowth",
                table: "KoiGrowth");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedingSchedule",
                table: "FeedingSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedCalculation",
                table: "FeedCalculation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogPost",
                table: "BlogPost");

            migrationBuilder.RenameTable(
                name: "WaterParameter",
                newName: "WaterParameters");

            migrationBuilder.RenameTable(
                name: "SaltRequirement",
                newName: "SaltRequirements");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "Pond",
                newName: "Ponds");

            migrationBuilder.RenameTable(
                name: "PerfectWaterVolume",
                newName: "PerfectWaterVolumes");

            migrationBuilder.RenameTable(
                name: "OrderDetail",
                newName: "OrderDetails");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "KoiType",
                newName: "KoiTypes");

            migrationBuilder.RenameTable(
                name: "KoiIndividual",
                newName: "KoiIndividuals");

            migrationBuilder.RenameTable(
                name: "KoiGrowth",
                newName: "KoiGrowths");

            migrationBuilder.RenameTable(
                name: "FeedingSchedule",
                newName: "FeedingSchedules");

            migrationBuilder.RenameTable(
                name: "FeedCalculation",
                newName: "FeedCalculations");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "BlogPost",
                newName: "BlogPosts");

            migrationBuilder.RenameIndex(
                name: "IX_WaterParameter_PondId",
                table: "WaterParameters",
                newName: "IX_WaterParameters_PondId");

            migrationBuilder.RenameIndex(
                name: "IX_SaltRequirement_PondId",
                table: "SaltRequirements",
                newName: "IX_SaltRequirements_PondId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CategoryId",
                table: "Products",
                newName: "IX_Products_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Pond_OwnerId",
                table: "Ponds",
                newName: "IX_Ponds_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_PerfectWaterVolume_KoiTypeId",
                table: "PerfectWaterVolumes",
                newName: "IX_PerfectWaterVolumes_KoiTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_ProductId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_CustomerId",
                table: "Orders",
                newName: "IX_Orders_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_KoiIndividual_PondId",
                table: "KoiIndividuals",
                newName: "IX_KoiIndividuals_PondId");

            migrationBuilder.RenameIndex(
                name: "IX_KoiIndividual_KoiTypeId",
                table: "KoiIndividuals",
                newName: "IX_KoiIndividuals_KoiTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_KoiIndividual_CategoryId",
                table: "KoiIndividuals",
                newName: "IX_KoiIndividuals_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_KoiGrowth_KoiIndividualId",
                table: "KoiGrowths",
                newName: "IX_KoiGrowths_KoiIndividualId");

            migrationBuilder.RenameIndex(
                name: "IX_FeedingSchedule_KoiIndividualId",
                table: "FeedingSchedules",
                newName: "IX_FeedingSchedules_KoiIndividualId");

            migrationBuilder.RenameIndex(
                name: "IX_FeedCalculation_KoiIndividualId",
                table: "FeedCalculations",
                newName: "IX_FeedCalculations_KoiIndividualId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogPost_AuthorId",
                table: "BlogPosts",
                newName: "IX_BlogPosts_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WaterParameters",
                table: "WaterParameters",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaltRequirements",
                table: "SaltRequirements",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ponds",
                table: "Ponds",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PerfectWaterVolumes",
                table: "PerfectWaterVolumes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KoiTypes",
                table: "KoiTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KoiIndividuals",
                table: "KoiIndividuals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KoiGrowths",
                table: "KoiGrowths",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedingSchedules",
                table: "FeedingSchedules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedCalculations",
                table: "FeedCalculations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogPosts",
                table: "BlogPosts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_Users_AuthorId",
                table: "BlogPosts",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedCalculations_KoiIndividuals_KoiIndividualId",
                table: "FeedCalculations",
                column: "KoiIndividualId",
                principalTable: "KoiIndividuals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedingSchedules_KoiIndividuals_KoiIndividualId",
                table: "FeedingSchedules",
                column: "KoiIndividualId",
                principalTable: "KoiIndividuals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KoiGrowths_KoiIndividuals_KoiIndividualId",
                table: "KoiGrowths",
                column: "KoiIndividualId",
                principalTable: "KoiIndividuals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KoiIndividuals_Categories_CategoryId",
                table: "KoiIndividuals",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KoiIndividuals_KoiTypes_KoiTypeId",
                table: "KoiIndividuals",
                column: "KoiTypeId",
                principalTable: "KoiTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KoiIndividuals_Ponds_PondId",
                table: "KoiIndividuals",
                column: "PondId",
                principalTable: "Ponds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PerfectWaterVolumes_KoiTypes_KoiTypeId",
                table: "PerfectWaterVolumes",
                column: "KoiTypeId",
                principalTable: "KoiTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ponds_Users_OwnerId",
                table: "Ponds",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SaltRequirements_Ponds_PondId",
                table: "SaltRequirements",
                column: "PondId",
                principalTable: "Ponds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WaterParameters_Ponds_PondId",
                table: "WaterParameters",
                column: "PondId",
                principalTable: "Ponds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_Users_AuthorId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedCalculations_KoiIndividuals_KoiIndividualId",
                table: "FeedCalculations");

            migrationBuilder.DropForeignKey(
                name: "FK_FeedingSchedules_KoiIndividuals_KoiIndividualId",
                table: "FeedingSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_KoiGrowths_KoiIndividuals_KoiIndividualId",
                table: "KoiGrowths");

            migrationBuilder.DropForeignKey(
                name: "FK_KoiIndividuals_Categories_CategoryId",
                table: "KoiIndividuals");

            migrationBuilder.DropForeignKey(
                name: "FK_KoiIndividuals_KoiTypes_KoiTypeId",
                table: "KoiIndividuals");

            migrationBuilder.DropForeignKey(
                name: "FK_KoiIndividuals_Ponds_PondId",
                table: "KoiIndividuals");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PerfectWaterVolumes_KoiTypes_KoiTypeId",
                table: "PerfectWaterVolumes");

            migrationBuilder.DropForeignKey(
                name: "FK_Ponds_Users_OwnerId",
                table: "Ponds");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_SaltRequirements_Ponds_PondId",
                table: "SaltRequirements");

            migrationBuilder.DropForeignKey(
                name: "FK_WaterParameters_Ponds_PondId",
                table: "WaterParameters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WaterParameters",
                table: "WaterParameters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaltRequirements",
                table: "SaltRequirements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ponds",
                table: "Ponds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PerfectWaterVolumes",
                table: "PerfectWaterVolumes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KoiTypes",
                table: "KoiTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KoiIndividuals",
                table: "KoiIndividuals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KoiGrowths",
                table: "KoiGrowths");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedingSchedules",
                table: "FeedingSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedCalculations",
                table: "FeedCalculations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogPosts",
                table: "BlogPosts");

            migrationBuilder.RenameTable(
                name: "WaterParameters",
                newName: "WaterParameter");

            migrationBuilder.RenameTable(
                name: "SaltRequirements",
                newName: "SaltRequirement");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameTable(
                name: "Ponds",
                newName: "Pond");

            migrationBuilder.RenameTable(
                name: "PerfectWaterVolumes",
                newName: "PerfectWaterVolume");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameTable(
                name: "OrderDetails",
                newName: "OrderDetail");

            migrationBuilder.RenameTable(
                name: "KoiTypes",
                newName: "KoiType");

            migrationBuilder.RenameTable(
                name: "KoiIndividuals",
                newName: "KoiIndividual");

            migrationBuilder.RenameTable(
                name: "KoiGrowths",
                newName: "KoiGrowth");

            migrationBuilder.RenameTable(
                name: "FeedingSchedules",
                newName: "FeedingSchedule");

            migrationBuilder.RenameTable(
                name: "FeedCalculations",
                newName: "FeedCalculation");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameTable(
                name: "BlogPosts",
                newName: "BlogPost");

            migrationBuilder.RenameIndex(
                name: "IX_WaterParameters_PondId",
                table: "WaterParameter",
                newName: "IX_WaterParameter_PondId");

            migrationBuilder.RenameIndex(
                name: "IX_SaltRequirements_PondId",
                table: "SaltRequirement",
                newName: "IX_SaltRequirement_PondId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryId",
                table: "Product",
                newName: "IX_Product_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Ponds_OwnerId",
                table: "Pond",
                newName: "IX_Pond_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_PerfectWaterVolumes_KoiTypeId",
                table: "PerfectWaterVolume",
                newName: "IX_PerfectWaterVolume_KoiTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_CustomerId",
                table: "Order",
                newName: "IX_Order_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetail",
                newName: "IX_OrderDetail_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetail",
                newName: "IX_OrderDetail_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_KoiIndividuals_PondId",
                table: "KoiIndividual",
                newName: "IX_KoiIndividual_PondId");

            migrationBuilder.RenameIndex(
                name: "IX_KoiIndividuals_KoiTypeId",
                table: "KoiIndividual",
                newName: "IX_KoiIndividual_KoiTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_KoiIndividuals_CategoryId",
                table: "KoiIndividual",
                newName: "IX_KoiIndividual_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_KoiGrowths_KoiIndividualId",
                table: "KoiGrowth",
                newName: "IX_KoiGrowth_KoiIndividualId");

            migrationBuilder.RenameIndex(
                name: "IX_FeedingSchedules_KoiIndividualId",
                table: "FeedingSchedule",
                newName: "IX_FeedingSchedule_KoiIndividualId");

            migrationBuilder.RenameIndex(
                name: "IX_FeedCalculations_KoiIndividualId",
                table: "FeedCalculation",
                newName: "IX_FeedCalculation_KoiIndividualId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogPosts_AuthorId",
                table: "BlogPost",
                newName: "IX_BlogPost_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WaterParameter",
                table: "WaterParameter",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaltRequirement",
                table: "SaltRequirement",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pond",
                table: "Pond",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PerfectWaterVolume",
                table: "PerfectWaterVolume",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KoiType",
                table: "KoiType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KoiIndividual",
                table: "KoiIndividual",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KoiGrowth",
                table: "KoiGrowth",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedingSchedule",
                table: "FeedingSchedule",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedCalculation",
                table: "FeedCalculation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogPost",
                table: "BlogPost",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPost_Users_AuthorId",
                table: "BlogPost",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedCalculation_KoiIndividual_KoiIndividualId",
                table: "FeedCalculation",
                column: "KoiIndividualId",
                principalTable: "KoiIndividual",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeedingSchedule_KoiIndividual_KoiIndividualId",
                table: "FeedingSchedule",
                column: "KoiIndividualId",
                principalTable: "KoiIndividual",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KoiGrowth_KoiIndividual_KoiIndividualId",
                table: "KoiGrowth",
                column: "KoiIndividualId",
                principalTable: "KoiIndividual",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KoiIndividual_Category_CategoryId",
                table: "KoiIndividual",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KoiIndividual_KoiType_KoiTypeId",
                table: "KoiIndividual",
                column: "KoiTypeId",
                principalTable: "KoiType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KoiIndividual_Pond_PondId",
                table: "KoiIndividual",
                column: "PondId",
                principalTable: "Pond",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Users_CustomerId",
                table: "Order",
                column: "CustomerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Order_OrderId",
                table: "OrderDetail",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Product_ProductId",
                table: "OrderDetail",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PerfectWaterVolume_KoiType_KoiTypeId",
                table: "PerfectWaterVolume",
                column: "KoiTypeId",
                principalTable: "KoiType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pond_Users_OwnerId",
                table: "Pond",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Product",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SaltRequirement_Pond_PondId",
                table: "SaltRequirement",
                column: "PondId",
                principalTable: "Pond",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WaterParameter_Pond_PondId",
                table: "WaterParameter",
                column: "PondId",
                principalTable: "Pond",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
