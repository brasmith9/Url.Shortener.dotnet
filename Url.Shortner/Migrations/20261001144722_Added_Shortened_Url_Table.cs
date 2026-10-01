using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Url.Shortner.Migrations
{
    /// <inheritdoc />
    public partial class Added_Shortened_Url_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shortened_urls",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    short_url = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shortened_urls", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_shortened_urls_code",
                table: "shortened_urls",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shortened_urls");
        }
    }
}
