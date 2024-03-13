using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CertificateManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Mg1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "certificates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    certificate_data = table.Column<byte[]>(type: "bytea", nullable: false),
                    created_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                    last_modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_certificates", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    age = table.Column<int>(type: "integer", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    has_certificate = table.Column<bool>(type: "boolean", nullable: false),
                    certificate_id = table.Column<Guid>(type: "uuid", nullable: true),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    user_role = table.Column<int>(type: "integer", nullable: false),
                    created_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_modified_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                    last_modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_certificates_certificate_id",
                        column: x => x.certificate_id,
                        principalTable: "certificates",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_users_certificate_id",
                table: "users",
                column: "certificate_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "certificates");
        }
    }
}
