using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SudokuDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CreateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameBoards",
                columns: table => new
                {
                    GameBoardId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsKillerSudoku = table.Column<bool>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameBoards", x => x.GameBoardId);
                });

            migrationBuilder.CreateTable(
                name: "Icons",
                columns: table => new
                {
                    IconId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Icons", x => x.IconId);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    OptionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.OptionId);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Row = table.Column<int>(type: "INTEGER", nullable: false),
                    Column = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.PositionId);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    UserProfileId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IconId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.UserProfileId);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Icons_IconId",
                        column: x => x.IconId,
                        principalTable: "Icons",
                        principalColumn: "IconId");
                });

            migrationBuilder.CreateTable(
                name: "GameBoardValues",
                columns: table => new
                {
                    GameBoardValueId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<int>(type: "INTEGER", nullable: true),
                    GameBoardId = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    TimeRecorded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    GameBoardId = table.Column<int>(type: "INTEGER", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "GameRuns",
                columns: table => new
                {
                    GameRunId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    TimeRecorded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MillisecondsTaken = table.Column<long>(type: "INTEGER", nullable: true),
                    WasCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRuns", x => x.GameRunId);
                    table.ForeignKey(
                        name: "FK_GameRuns_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameRunHistoryEntry",
                columns: table => new
                {
                    GameRunHistoryEntryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PreviousValue = table.Column<int>(type: "INTEGER", nullable: true),
                    NewValue = table.Column<int>(type: "INTEGER", nullable: true),
                    GameRunId = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRunHistoryEntry", x => x.GameRunHistoryEntryId);
                    table.ForeignKey(
                        name: "FK_GameRunHistoryEntry_GameRuns_GameRunId",
                        column: x => x.GameRunId,
                        principalTable: "GameRuns",
                        principalColumn: "GameRunId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameRunHistoryEntry_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "PositionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameRunOptions",
                columns: table => new
                {
                    GameRunOptionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<bool>(type: "INTEGER", nullable: false),
                    GameRunId = table.Column<int>(type: "INTEGER", nullable: false),
                    OptionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRunOptions", x => x.GameRunOptionId);
                    table.ForeignKey(
                        name: "FK_GameRunOptions_GameRuns_GameRunId",
                        column: x => x.GameRunId,
                        principalTable: "GameRuns",
                        principalColumn: "GameRunId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameRunOptions_Options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Options",
                        principalColumn: "OptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameRunValues",
                columns: table => new
                {
                    GameRunValueId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<int>(type: "INTEGER", nullable: true),
                    GameRunId = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRunValues", x => x.GameRunValueId);
                    table.ForeignKey(
                        name: "FK_GameRunValues_GameRuns_GameRunId",
                        column: x => x.GameRunId,
                        principalTable: "GameRuns",
                        principalColumn: "GameRunId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameRunValues_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "PositionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Icons",
                columns: new[] { "IconId", "Path", "Title" },
                values: new object[,]
                {
                    { 1, "img/1669707620smiley-classic-icon_grey.svg", "Guest" },
                    { 2, "img/1669707620smiley-classic-icon_brown.svg", "Brown" },
                    { 3, "img/1669707620smiley-classic-icon_pink.svg", "Pink" },
                    { 4, "img/1669707620smiley-classic-icon_orange.svg", "Orange" },
                    { 5, "img/1669707620smiley-classic-icon_yellow.svg", "Yellow" },
                    { 6, "img/1669707620smiley-classic-icon_green.svg", "Green" },
                    { 7, "img/1669707620smiley-classic-icon_cyan.svg", "Cyan" },
                    { 8, "img/1669707620smiley-classic-icon_purple.svg", "Purple" }
                });

            migrationBuilder.InsertData(
                table: "Options",
                columns: new[] { "OptionId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Show possible values for each square", "ShowPossibleValues" },
                    { 2, "Highlight squares with only one possible value in pale yellow", "HighlightSinglePossibleValue" },
                    { 3, "ighlight squares with possible value unique in row, column, square or diagonal in pale orange", "HighlightUniquePossibleValueInGroup" }
                });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "PositionId", "Column", "Row" },
                values: new object[,]
                {
                    { 1, 0, 0 },
                    { 2, 1, 0 },
                    { 3, 2, 0 },
                    { 4, 3, 0 },
                    { 5, 4, 0 },
                    { 6, 5, 0 },
                    { 7, 6, 0 },
                    { 8, 7, 0 },
                    { 9, 8, 0 },
                    { 10, 0, 1 },
                    { 11, 1, 1 },
                    { 12, 2, 1 },
                    { 13, 3, 1 },
                    { 14, 4, 1 },
                    { 15, 5, 1 },
                    { 16, 6, 1 },
                    { 17, 7, 1 },
                    { 18, 8, 1 },
                    { 19, 0, 2 },
                    { 20, 1, 2 },
                    { 21, 2, 2 },
                    { 22, 3, 2 },
                    { 23, 4, 2 },
                    { 24, 5, 2 },
                    { 25, 6, 2 },
                    { 26, 7, 2 },
                    { 27, 8, 2 },
                    { 28, 0, 3 },
                    { 29, 1, 3 },
                    { 30, 2, 3 },
                    { 31, 3, 3 },
                    { 32, 4, 3 },
                    { 33, 5, 3 },
                    { 34, 6, 3 },
                    { 35, 7, 3 },
                    { 36, 8, 3 },
                    { 37, 0, 4 },
                    { 38, 1, 4 },
                    { 39, 2, 4 },
                    { 40, 3, 4 },
                    { 41, 4, 4 },
                    { 42, 5, 4 },
                    { 43, 6, 4 },
                    { 44, 7, 4 },
                    { 45, 8, 4 },
                    { 46, 0, 5 },
                    { 47, 1, 5 },
                    { 48, 2, 5 },
                    { 49, 3, 5 },
                    { 50, 4, 5 },
                    { 51, 5, 5 },
                    { 52, 6, 5 },
                    { 53, 7, 5 },
                    { 54, 8, 5 },
                    { 55, 0, 6 },
                    { 56, 1, 6 },
                    { 57, 2, 6 },
                    { 58, 3, 6 },
                    { 59, 4, 6 },
                    { 60, 5, 6 },
                    { 61, 6, 6 },
                    { 62, 7, 6 },
                    { 63, 8, 6 },
                    { 64, 0, 7 },
                    { 65, 1, 7 },
                    { 66, 2, 7 },
                    { 67, 3, 7 },
                    { 68, 4, 7 },
                    { 69, 5, 7 },
                    { 70, 6, 7 },
                    { 71, 7, 7 },
                    { 72, 8, 7 },
                    { 73, 0, 8 },
                    { 74, 1, 8 },
                    { 75, 2, 8 },
                    { 76, 3, 8 },
                    { 77, 4, 8 },
                    { 78, 5, 8 },
                    { 79, 6, 8 },
                    { 80, 7, 8 },
                    { 81, 8, 8 }
                });

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "UserProfileId", "IconId", "Name" },
                values: new object[] { 1, 1, "Guest" });

            migrationBuilder.CreateIndex(
                name: "IX_GameBoardValues_GameBoardId",
                table: "GameBoardValues",
                column: "GameBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_GameBoardValues_PositionId",
                table: "GameBoardValues",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRunHistoryEntry_GameRunId",
                table: "GameRunHistoryEntry",
                column: "GameRunId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRunHistoryEntry_PositionId",
                table: "GameRunHistoryEntry",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRunOptions_GameRunId",
                table: "GameRunOptions",
                column: "GameRunId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRunOptions_OptionId",
                table: "GameRunOptions",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRuns_GameId",
                table: "GameRuns",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRunValues_GameRunId",
                table: "GameRunValues",
                column: "GameRunId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRunValues_PositionId",
                table: "GameRunValues",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameBoardId",
                table: "Games",
                column: "GameBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_UserProfileId",
                table: "Games",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_IconId",
                table: "UserProfiles",
                column: "IconId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameBoardValues");

            migrationBuilder.DropTable(
                name: "GameRunHistoryEntry");

            migrationBuilder.DropTable(
                name: "GameRunOptions");

            migrationBuilder.DropTable(
                name: "GameRunValues");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropTable(
                name: "GameRuns");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "GameBoards");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Icons");
        }
    }
}
