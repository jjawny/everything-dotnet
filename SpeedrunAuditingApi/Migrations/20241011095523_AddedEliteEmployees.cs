using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpeedrunAuditingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedEliteEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EliteEmployees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsElite = table.Column<bool>(type: "INTEGER", nullable: false),
                    Audit_CreatedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    Audit_UpdatedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Audit_UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Audit_IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EliteEmployees", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EliteEmployees");
        }
    }
}
