using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SudokuDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ReorganiseGameModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameRuns_Games_GameId",
                table: "GameRuns");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Icons_IconId",
                table: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "GameRuns");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "GameRuns",
                newName: "UserGameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameRuns_GameId",
                table: "GameRuns",
                newName: "IX_GameRuns_UserGameId");

            migrationBuilder.AlterColumn<int>(
                name: "IconId",
                table: "UserProfiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RunNumber",
                table: "GameRuns",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserGames",
                columns: table => new
                {
                    UserGameId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    GameBoardId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGames", x => x.UserGameId);
                    table.ForeignKey(
                        name: "FK_UserGames_GameBoards_GameBoardId",
                        column: x => x.GameBoardId,
                        principalTable: "GameBoards",
                        principalColumn: "GameBoardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGames_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserGames_GameBoardId",
                table: "UserGames",
                column: "GameBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGames_UserProfileId",
                table: "UserGames",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameRuns_UserGames_UserGameId",
                table: "GameRuns",
                column: "UserGameId",
                principalTable: "UserGames",
                principalColumn: "UserGameId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Icons_IconId",
                table: "UserProfiles",
                column: "IconId",
                principalTable: "Icons",
                principalColumn: "IconId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameRuns_UserGames_UserGameId",
                table: "GameRuns");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Icons_IconId",
                table: "UserProfiles");

            migrationBuilder.DropTable(
                name: "UserGames");

            migrationBuilder.DropColumn(
                name: "RunNumber",
                table: "GameRuns");

            migrationBuilder.RenameColumn(
                name: "UserGameId",
                table: "GameRuns",
                newName: "GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameRuns_UserGameId",
                table: "GameRuns",
                newName: "IX_GameRuns_GameId");

            migrationBuilder.AlterColumn<int>(
                name: "IconId",
                table: "UserProfiles",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "GameRuns",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameBoardId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeRecorded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_Games_GameBoards_GameBoardId",
                        column: x => x.GameBoardId,
                        principalTable: "GameBoards",
                        principalColumn: "GameBoardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameBoardId",
                table: "Games",
                column: "GameBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_UserProfileId",
                table: "Games",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameRuns_Games_GameId",
                table: "GameRuns",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Icons_IconId",
                table: "UserProfiles",
                column: "IconId",
                principalTable: "Icons",
                principalColumn: "IconId");
        }
    }
}
