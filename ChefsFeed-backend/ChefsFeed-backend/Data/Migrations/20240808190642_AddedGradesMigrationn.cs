using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChefsFeed_backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedGradesMigrationn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserGrades",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RecipeId = table.Column<long>(type: "bigint", nullable: false),
                    Grade = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGrades", x => new { x.UserId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_UserGrades_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGrades_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserGrades_RecipeId",
                table: "UserGrades",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserGrades");
        }
    }
}
