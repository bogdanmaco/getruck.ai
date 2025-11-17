using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace loadmaster_api.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Trucks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Trailers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Loads",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Drivers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Customers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_TenantId",
                table: "Trucks",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Trailers_TenantId",
                table: "Trailers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Loads_TenantId",
                table: "Loads",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_TenantId",
                table: "Drivers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TenantId",
                table: "Customers",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Tenants_TenantId",
                table: "Customers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Tenants_TenantId",
                table: "Drivers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loads_Tenants_TenantId",
                table: "Loads",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trailers_Tenants_TenantId",
                table: "Trailers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trucks_Tenants_TenantId",
                table: "Trucks",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Tenants_TenantId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Tenants_TenantId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Loads_Tenants_TenantId",
                table: "Loads");

            migrationBuilder.DropForeignKey(
                name: "FK_Trailers_Tenants_TenantId",
                table: "Trailers");

            migrationBuilder.DropForeignKey(
                name: "FK_Trucks_Tenants_TenantId",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_Trucks_TenantId",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_Trailers_TenantId",
                table: "Trailers");

            migrationBuilder.DropIndex(
                name: "IX_Loads_TenantId",
                table: "Loads");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_TenantId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_TenantId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Trailers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Customers");
        }
    }
}
