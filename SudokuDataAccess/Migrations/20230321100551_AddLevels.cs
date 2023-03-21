using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SudokuDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddLevels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "GameBoards",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Level",
                columns: table => new
                {
                    LevelId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    MinimumNumberOfInitialValues = table.Column<int>(type: "INTEGER", nullable: false),
                    MaximumNumberOfInitialValues = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Level", x => x.LevelId);
                });

            migrationBuilder.InsertData(
                table: "Level",
                columns: new[] { "LevelId", "MaximumNumberOfInitialValues", "MinimumNumberOfInitialValues", "Title" },
                values: new object[,]
                {
                    { 1, 81, 36, "Novice" },
                    { 2, 35, 32, "Moderate" },
                    { 3, 31, 28, "Hard" },
                    { 4, 27, 24, "Very Hard" },
                    { 5, 23, 1, "Genius" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameBoards_LevelId",
                table: "GameBoards",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameBoards_Level_LevelId",
                table: "GameBoards",
                column: "LevelId",
                principalTable: "Level",
                principalColumn: "LevelId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameBoards_Level_LevelId",
                table: "GameBoards");

            migrationBuilder.DropTable(
                name: "Level");

            migrationBuilder.DropIndex(
                name: "IX_GameBoards_LevelId",
                table: "GameBoards");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "GameBoards");
        }
    }
}
