using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalDiary.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHomePageToolsConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_HomePages_UserId",
                table: "HomePages",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HomePages_Users_UserId",
                table: "HomePages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomePages_Users_UserId",
                table: "HomePages");

            migrationBuilder.DropIndex(
                name: "IX_HomePages_UserId",
                table: "HomePages");
        }
    }
}
