using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SomaShare.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Textbooks_Author",
                table: "Textbooks",
                column: "Author");

            migrationBuilder.CreateIndex(
                name: "IX_Textbooks_Campus",
                table: "Textbooks",
                column: "Campus");

            migrationBuilder.CreateIndex(
                name: "IX_Textbooks_ISBN",
                table: "Textbooks",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_Textbooks_Price",
                table: "Textbooks",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Textbooks_Title",
                table: "Textbooks",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FullName",
                table: "AspNetUsers",
                column: "FullName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Textbooks_Author",
                table: "Textbooks");

            migrationBuilder.DropIndex(
                name: "IX_Textbooks_Campus",
                table: "Textbooks");

            migrationBuilder.DropIndex(
                name: "IX_Textbooks_ISBN",
                table: "Textbooks");

            migrationBuilder.DropIndex(
                name: "IX_Textbooks_Price",
                table: "Textbooks");

            migrationBuilder.DropIndex(
                name: "IX_Textbooks_Title",
                table: "Textbooks");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FullName",
                table: "AspNetUsers");
        }
    }
}
