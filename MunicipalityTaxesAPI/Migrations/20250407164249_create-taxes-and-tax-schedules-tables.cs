using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MunicipalityTaxesAPI.Migrations
{
    /// <inheritdoc />
    public partial class createtaxesandtaxschedulestables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "taxes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    municipality = table.Column<string>(type: "varchar(255)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    rate = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taxes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tax_schedules",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    period_start = table.Column<DateTime>(type: "datetime", nullable: false),
                    period_end = table.Column<DateTime>(type: "datetime", nullable: false),
                    tax_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tax_schedules", x => x.id);
                    table.ForeignKey(
                        name: "FK_tax_schedules_taxes_tax_id",
                        column: x => x.tax_id,
                        principalTable: "taxes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tax_schedules_tax_id",
                table: "tax_schedules",
                column: "tax_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tax_schedules");

            migrationBuilder.DropTable(
                name: "taxes");
        }
    }
}
