using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SudokuDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameBoardToGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameRunHistoryEntry_GameRuns_GameRunId",
                table: "GameRunHistoryEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_GameRunHistoryEntry_Positions_PositionId",
                table: "GameRunHistoryEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGames_GameBoards_GameBoardId",
                table: "UserGames");

            migrationBuilder.DropTable(
                name: "GameBoardValues");

            migrationBuilder.DropTable(
                name: "GameBoards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameRunHistoryEntry",
                table: "GameRunHistoryEntry");

            migrationBuilder.RenameTable(
                name: "GameRunHistoryEntry",
                newName: "GameRunHistoryEntries");

            migrationBuilder.RenameColumn(
                name: "GameBoardId",
                table: "UserGames",
                newName: "GameId");

            migrationBuilder.RenameIndex(
                name: "IX_UserGames_GameBoardId",
                table: "UserGames",
                newName: "IX_UserGames_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameRunHistoryEntry_PositionId",
                table: "GameRunHistoryEntries",
                newName: "IX_GameRunHistoryEntries_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_GameRunHistoryEntry_GameRunId",
                table: "GameRunHistoryEntries",
                newName: "IX_GameRunHistoryEntries_GameRunId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameRunHistoryEntries",
                table: "GameRunHistoryEntries",
                column: "GameRunHistoryEntryId");

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsKillerSudoku = table.Column<bool>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    TimeRecorded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Hash = table.Column<int>(type: "INTEGER", nullable: false),
                    LevelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                    table.ForeignKey(
                        name: "FK_Games_Level_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Level",
                        principalColumn: "LevelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameInitialValues",
                columns: table => new
                {
                    GameInitialValueId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<int>(type: "INTEGER", nullable: true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameInitialValues", x => x.GameInitialValueId);
                    table.ForeignKey(
                        name: "FK_GameInitialValues_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameInitialValues_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "PositionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Options",
                keyColumn: "OptionId",
                keyValue: 3,
                column: "Description",
                value: "Highlight squares with possible value unique in row, column, square or diagonal in pale orange");

            migrationBuilder.CreateIndex(
                name: "IX_GameInitialValues_GameId",
                table: "GameInitialValues",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameInitialValues_PositionId",
                table: "GameInitialValues",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_LevelId",
                table: "Games",
                column: "LevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameRunHistoryEntries_GameRuns_GameRunId",
                table: "GameRunHistoryEntries",
                column: "GameRunId",
                principalTable: "GameRuns",
                principalColumn: "GameRunId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameRunHistoryEntries_Positions_PositionId",
                table: "GameRunHistoryEntries",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "PositionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGames_Games_GameId",
                table: "UserGames",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameRunHistoryEntries_GameRuns_GameRunId",
                table: "GameRunHistoryEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_GameRunHistoryEntries_Positions_PositionId",
                table: "GameRunHistoryEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGames_Games_GameId",
                table: "UserGames");

            migrationBuilder.DropTable(
                name: "GameInitialValues");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameRunHistoryEntries",
                table: "GameRunHistoryEntries");

            migrationBuilder.RenameTable(
                name: "GameRunHistoryEntries",
                newName: "GameRunHistoryEntry");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "UserGames",
                newName: "GameBoardId");

            migrationBuilder.RenameIndex(
                name: "IX_UserGames_GameId",
                table: "UserGames",
                newName: "IX_UserGames_GameBoardId");

            migrationBuilder.RenameIndex(
                name: "IX_GameRunHistoryEntries_PositionId",
                table: "GameRunHistoryEntry",
                newName: "IX_GameRunHistoryEntry_PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_GameRunHistoryEntries_GameRunId",
                table: "GameRunHistoryEntry",
                newName: "IX_GameRunHistoryEntry_GameRunId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameRunHistoryEntry",
                table: "GameRunHistoryEntry",
                column: "GameRunHistoryEntryId");

            migrationBuilder.CreateTable(
                name: "GameBoards",
                columns: table => new
                {
                    GameBoardId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LevelId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsKillerSudoku = table.Column<bool>(type: "INTEGER", nullable: false),
                    TimeRecorded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameBoards", x => x.GameBoardId);
                    table.ForeignKey(
                        name: "FK_GameBoards_Level_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Level",
                        principalColumn: "LevelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameBoardValues",
                columns: table => new
                {
                    GameBoardValueId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameBoardId = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameBoardValues", x => x.GameBoardValueId);
                    table.ForeignKey(
                        name: "FK_GameBoardValues_GameBoards_GameBoardId",
                        column: x => x.GameBoardId,
                        principalTable: "GameBoards",
                        principalColumn: "GameBoardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameBoardValues_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "PositionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Options",
                keyColumn: "OptionId",
                keyValue: 3,
                column: "Description",
                value: "ighlight squares with possible value unique in row, column, square or diagonal in pale orange");

            migrationBuilder.CreateIndex(
                name: "IX_GameBoards_LevelId",
                table: "GameBoards",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_GameBoardValues_GameBoardId",
                table: "GameBoardValues",
                column: "GameBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_GameBoardValues_PositionId",
                table: "GameBoardValues",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameRunHistoryEntry_GameRuns_GameRunId",
                table: "GameRunHistoryEntry",
                column: "GameRunId",
                principalTable: "GameRuns",
                principalColumn: "GameRunId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameRunHistoryEntry_Positions_PositionId",
                table: "GameRunHistoryEntry",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "PositionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGames_GameBoards_GameBoardId",
                table: "UserGames",
                column: "GameBoardId",
                principalTable: "GameBoards",
                principalColumn: "GameBoardId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
