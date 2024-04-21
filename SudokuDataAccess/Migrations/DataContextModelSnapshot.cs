﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SudokuDataAccess;

#nullable disable

namespace SudokuDataAccess.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.1");

            modelBuilder.Entity("SudokuDataAccess.Models.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Hash")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsKillerSudoku")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LevelId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("TimeRecorded")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("GameId");

                    b.HasIndex("LevelId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.GameInitialValue", b =>
                {
                    b.Property<int>("GameInitialValueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PositionId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Value")
                        .HasColumnType("INTEGER");

                    b.HasKey("GameInitialValueId");

                    b.HasIndex("GameId");

                    b.HasIndex("PositionId");

                    b.ToTable("GameInitialValues");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.GameRun", b =>
                {
                    b.Property<int>("GameRunId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("MillisecondsTaken")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RunNumber")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("TimeRecorded")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserGameId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("WasCompleted")
                        .HasColumnType("INTEGER");

                    b.HasKey("GameRunId");

                    b.HasIndex("UserGameId");

                    b.ToTable("GameRuns");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.GameRunHistoryEntry", b =>
                {
                    b.Property<int>("GameRunHistoryEntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameRunId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("NewValue")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PositionId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PreviousValue")
                        .HasColumnType("INTEGER");

                    b.HasKey("GameRunHistoryEntryId");

                    b.HasIndex("GameRunId");

                    b.HasIndex("PositionId");

                    b.ToTable("GameRunHistoryEntries");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.GameRunOption", b =>
                {
                    b.Property<int>("GameRunOptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameRunId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OptionId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Value")
                        .HasColumnType("INTEGER");

                    b.HasKey("GameRunOptionId");

                    b.HasIndex("GameRunId");

                    b.HasIndex("OptionId");

                    b.ToTable("GameRunOptions");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.GameRunValue", b =>
                {
                    b.Property<int>("GameRunValueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameRunId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PositionId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Value")
                        .HasColumnType("INTEGER");

                    b.HasKey("GameRunValueId");

                    b.HasIndex("GameRunId");

                    b.HasIndex("PositionId");

                    b.ToTable("GameRunValues");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.Reference.Icon", b =>
                {
                    b.Property<int>("IconId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IconId");

                    b.ToTable("Icons");

                    b.HasData(
                        new
                        {
                            IconId = 1,
                            Path = "img/1669707620smiley-classic-icon_grey.svg",
                            Title = "Guest"
                        },
                        new
                        {
                            IconId = 2,
                            Path = "img/1669707620smiley-classic-icon_brown.svg",
                            Title = "Brown"
                        },
                        new
                        {
                            IconId = 3,
                            Path = "img/1669707620smiley-classic-icon_pink.svg",
                            Title = "Pink"
                        },
                        new
                        {
                            IconId = 4,
                            Path = "img/1669707620smiley-classic-icon_orange.svg",
                            Title = "Orange"
                        },
                        new
                        {
                            IconId = 5,
                            Path = "img/1669707620smiley-classic-icon_yellow.svg",
                            Title = "Yellow"
                        },
                        new
                        {
                            IconId = 6,
                            Path = "img/1669707620smiley-classic-icon_green.svg",
                            Title = "Green"
                        },
                        new
                        {
                            IconId = 7,
                            Path = "img/1669707620smiley-classic-icon_cyan.svg",
                            Title = "Cyan"
                        },
                        new
                        {
                            IconId = 8,
                            Path = "img/1669707620smiley-classic-icon_purple.svg",
                            Title = "Purple"
                        });
                });

            modelBuilder.Entity("SudokuDataAccess.Models.Reference.Level", b =>
                {
                    b.Property<int>("LevelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaximumNumberOfInitialValues")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MinimumNumberOfInitialValues")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LevelId");

                    b.ToTable("Level");

                    b.HasData(
                        new
                        {
                            LevelId = 1,
                            MaximumNumberOfInitialValues = 81,
                            MinimumNumberOfInitialValues = 36,
                            Title = "Novice"
                        },
                        new
                        {
                            LevelId = 2,
                            MaximumNumberOfInitialValues = 35,
                            MinimumNumberOfInitialValues = 32,
                            Title = "Moderate"
                        },
                        new
                        {
                            LevelId = 3,
                            MaximumNumberOfInitialValues = 31,
                            MinimumNumberOfInitialValues = 28,
                            Title = "Hard"
                        },
                        new
                        {
                            LevelId = 4,
                            MaximumNumberOfInitialValues = 27,
                            MinimumNumberOfInitialValues = 24,
                            Title = "Very Hard"
                        },
                        new
                        {
                            LevelId = 5,
                            MaximumNumberOfInitialValues = 23,
                            MinimumNumberOfInitialValues = 1,
                            Title = "Genius"
                        });
                });

            modelBuilder.Entity("SudokuDataAccess.Models.Reference.Option", b =>
                {
                    b.Property<int>("OptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("OptionId");

                    b.ToTable("Options");

                    b.HasData(
                        new
                        {
                            OptionId = 1,
                            Description = "Show possible values for each square",
                            Name = "ShowPossibleValues"
                        },
                        new
                        {
                            OptionId = 2,
                            Description = "Highlight squares with only one possible value in pale yellow",
                            Name = "HighlightSinglePossibleValue"
                        },
                        new
                        {
                            OptionId = 3,
                            Description = "Highlight squares with possible value unique in row, column, square or diagonal in pale orange",
                            Name = "HighlightUniquePossibleValueInGroup"
                        });
                });

            modelBuilder.Entity("SudokuDataAccess.Models.Reference.Position", b =>
                {
                    b.Property<int>("PositionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Column")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Row")
                        .HasColumnType("INTEGER");

                    b.HasKey("PositionId");

                    b.ToTable("Positions");

                    b.HasData(
                        new
                        {
                            PositionId = 1,
                            Column = 0,
                            Row = 0
                        },
                        new
                        {
                            PositionId = 2,
                            Column = 1,
                            Row = 0
                        },
                        new
                        {
                            PositionId = 3,
                            Column = 2,
                            Row = 0
                        },
                        new
                        {
                            PositionId = 4,
                            Column = 3,
                            Row = 0
                        },
                        new
                        {
                            PositionId = 5,
                            Column = 4,
                            Row = 0
                        },
                        new
                        {
                            PositionId = 6,
                            Column = 5,
                            Row = 0
                        },
                        new
                        {
                            PositionId = 7,
                            Column = 6,
                            Row = 0
                        },
                        new
                        {
                            PositionId = 8,
                            Column = 7,
                            Row = 0
                        },
                        new
                        {
                            PositionId = 9,
                            Column = 8,
                            Row = 0
                        },
                        new
                        {
                            PositionId = 10,
                            Column = 0,
                            Row = 1
                        },
                        new
                        {
                            PositionId = 11,
                            Column = 1,
                            Row = 1
                        },
                        new
                        {
                            PositionId = 12,
                            Column = 2,
                            Row = 1
                        },
                        new
                        {
                            PositionId = 13,
                            Column = 3,
                            Row = 1
                        },
                        new
                        {
                            PositionId = 14,
                            Column = 4,
                            Row = 1
                        },
                        new
                        {
                            PositionId = 15,
                            Column = 5,
                            Row = 1
                        },
                        new
                        {
                            PositionId = 16,
                            Column = 6,
                            Row = 1
                        },
                        new
                        {
                            PositionId = 17,
                            Column = 7,
                            Row = 1
                        },
                        new
                        {
                            PositionId = 18,
                            Column = 8,
                            Row = 1
                        },
                        new
                        {
                            PositionId = 19,
                            Column = 0,
                            Row = 2
                        },
                        new
                        {
                            PositionId = 20,
                            Column = 1,
                            Row = 2
                        },
                        new
                        {
                            PositionId = 21,
                            Column = 2,
                            Row = 2
                        },
                        new
                        {
                            PositionId = 22,
                            Column = 3,
                            Row = 2
                        },
                        new
                        {
                            PositionId = 23,
                            Column = 4,
                            Row = 2
                        },
                        new
                        {
                            PositionId = 24,
                            Column = 5,
                            Row = 2
                        },
                        new
                        {
                            PositionId = 25,
                            Column = 6,
                            Row = 2
                        },
                        new
                        {
                            PositionId = 26,
                            Column = 7,
                            Row = 2
                        },
                        new
                        {
                            PositionId = 27,
                            Column = 8,
                            Row = 2
                        },
                        new
                        {
                            PositionId = 28,
                            Column = 0,
                            Row = 3
                        },
                        new
                        {
                            PositionId = 29,
                            Column = 1,
                            Row = 3
                        },
                        new
                        {
                            PositionId = 30,
                            Column = 2,
                            Row = 3
                        },
                        new
                        {
                            PositionId = 31,
                            Column = 3,
                            Row = 3
                        },
                        new
                        {
                            PositionId = 32,
                            Column = 4,
                            Row = 3
                        },
                        new
                        {
                            PositionId = 33,
                            Column = 5,
                            Row = 3
                        },
                        new
                        {
                            PositionId = 34,
                            Column = 6,
                            Row = 3
                        },
                        new
                        {
                            PositionId = 35,
                            Column = 7,
                            Row = 3
                        },
                        new
                        {
                            PositionId = 36,
                            Column = 8,
                            Row = 3
                        },
                        new
                        {
                            PositionId = 37,
                            Column = 0,
                            Row = 4
                        },
                        new
                        {
                            PositionId = 38,
                            Column = 1,
                            Row = 4
                        },
                        new
                        {
                            PositionId = 39,
                            Column = 2,
                            Row = 4
                        },
                        new
                        {
                            PositionId = 40,
                            Column = 3,
                            Row = 4
                        },
                        new
                        {
                            PositionId = 41,
                            Column = 4,
                            Row = 4
                        },
                        new
                        {
                            PositionId = 42,
                            Column = 5,
                            Row = 4
                        },
                        new
                        {
                            PositionId = 43,
                            Column = 6,
                            Row = 4
                        },
                        new
                        {
                            PositionId = 44,
                            Column = 7,
                            Row = 4
                        },
                        new
                        {
                            PositionId = 45,
                            Column = 8,
                            Row = 4
                        },
                        new
                        {
                            PositionId = 46,
                            Column = 0,
                            Row = 5
                        },
                        new
                        {
                            PositionId = 47,
                            Column = 1,
                            Row = 5
                        },
                        new
                        {
                            PositionId = 48,
                            Column = 2,
                            Row = 5
                        },
                        new
                        {
                            PositionId = 49,
                            Column = 3,
                            Row = 5
                        },
                        new
                        {
                            PositionId = 50,
                            Column = 4,
                            Row = 5
                        },
                        new
                        {
                            PositionId = 51,
                            Column = 5,
                            Row = 5
                        },
                        new
                        {
                            PositionId = 52,
                            Column = 6,
                            Row = 5
                        },
                        new
                        {
                            PositionId = 53,
                            Column = 7,
                            Row = 5
                        },
                        new
                        {
                            PositionId = 54,
                            Column = 8,
                            Row = 5
                        },
                        new
                        {
                            PositionId = 55,
                            Column = 0,
                            Row = 6
                        },
                        new
                        {
                            PositionId = 56,
                            Column = 1,
                            Row = 6
                        },
                        new
                        {
                            PositionId = 57,
                            Column = 2,
                            Row = 6
                        },
                        new
                        {
                            PositionId = 58,
                            Column = 3,
                            Row = 6
                        },
                        new
                        {
                            PositionId = 59,
                            Column = 4,
                            Row = 6
                        },
                        new
                        {
                            PositionId = 60,
                            Column = 5,
                            Row = 6
                        },
                        new
                        {
                            PositionId = 61,
                            Column = 6,
                            Row = 6
                        },
                        new
                        {
                            PositionId = 62,
                            Column = 7,
                            Row = 6
                        },
                        new
                        {
                            PositionId = 63,
                            Column = 8,
                            Row = 6
                        },
                        new
                        {
                            PositionId = 64,
                            Column = 0,
                            Row = 7
                        },
                        new
                        {
                            PositionId = 65,
                            Column = 1,
                            Row = 7
                        },
                        new
                        {
                            PositionId = 66,
                            Column = 2,
                            Row = 7
                        },
                        new
                        {
                            PositionId = 67,
                            Column = 3,
                            Row = 7
                        },
                        new
                        {
                            PositionId = 68,
                            Column = 4,
                            Row = 7
                        },
                        new
                        {
                            PositionId = 69,
                            Column = 5,
                            Row = 7
                        },
                        new
                        {
                            PositionId = 70,
                            Column = 6,
                            Row = 7
                        },
                        new
                        {
                            PositionId = 71,
                            Column = 7,
                            Row = 7
                        },
                        new
                        {
                            PositionId = 72,
                            Column = 8,
                            Row = 7
                        },
                        new
                        {
                            PositionId = 73,
                            Column = 0,
                            Row = 8
                        },
                        new
                        {
                            PositionId = 74,
                            Column = 1,
                            Row = 8
                        },
                        new
                        {
                            PositionId = 75,
                            Column = 2,
                            Row = 8
                        },
                        new
                        {
                            PositionId = 76,
                            Column = 3,
                            Row = 8
                        },
                        new
                        {
                            PositionId = 77,
                            Column = 4,
                            Row = 8
                        },
                        new
                        {
                            PositionId = 78,
                            Column = 5,
                            Row = 8
                        },
                        new
                        {
                            PositionId = 79,
                            Column = 6,
                            Row = 8
                        },
                        new
                        {
                            PositionId = 80,
                            Column = 7,
                            Row = 8
                        },
                        new
                        {
                            PositionId = 81,
                            Column = 8,
                            Row = 8
                        });
                });

            modelBuilder.Entity("SudokuDataAccess.Models.UserGame", b =>
                {
                    b.Property<int>("UserGameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserProfileId")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserGameId");

                    b.HasIndex("GameId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("UserGames");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.UserProfile", b =>
                {
                    b.Property<int>("UserProfileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IconId")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserProfileId");

                    b.HasIndex("IconId");

                    b.ToTable("UserProfiles");

                    b.HasData(
                        new
                        {
                            UserProfileId = 1,
                            IconId = 1,
                            Name = "Guest"
                        });
                });

            modelBuilder.Entity("SudokuDataAccess.Models.Game", b =>
                {
                    b.HasOne("SudokuDataAccess.Models.Reference.Level", "Level")
                        .WithMany()
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Level");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.GameInitialValue", b =>
                {
                    b.HasOne("SudokuDataAccess.Models.Game", "Game")
                        .WithMany("Values")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SudokuDataAccess.Models.Reference.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.GameRun", b =>
                {
                    b.HasOne("SudokuDataAccess.Models.UserGame", "Game")
                        .WithMany("Runs")
                        .HasForeignKey("UserGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.GameRunHistoryEntry", b =>
                {
                    b.HasOne("SudokuDataAccess.Models.GameRun", "GameRun")
                        .WithMany("History")
                        .HasForeignKey("GameRunId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SudokuDataAccess.Models.Reference.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameRun");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.GameRunOption", b =>
                {
                    b.HasOne("SudokuDataAccess.Models.GameRun", "GameRun")
                        .WithMany("Options")
                        .HasForeignKey("GameRunId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SudokuDataAccess.Models.Reference.Option", "Option")
                        .WithMany()
                        .HasForeignKey("OptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameRun");

                    b.Navigation("Option");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.GameRunValue", b =>
                {
                    b.HasOne("SudokuDataAccess.Models.GameRun", "GameRun")
                        .WithMany("Values")
                        .HasForeignKey("GameRunId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SudokuDataAccess.Models.Reference.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameRun");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.UserGame", b =>
                {
                    b.HasOne("SudokuDataAccess.Models.Game", "Game")
                        .WithMany("UserGames")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SudokuDataAccess.Models.UserProfile", "UserProfile")
                        .WithMany("Games")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.UserProfile", b =>
                {
                    b.HasOne("SudokuDataAccess.Models.Reference.Icon", "Icon")
                        .WithMany()
                        .HasForeignKey("IconId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Icon");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.Game", b =>
                {
                    b.Navigation("UserGames");

                    b.Navigation("Values");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.GameRun", b =>
                {
                    b.Navigation("History");

                    b.Navigation("Options");

                    b.Navigation("Values");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.UserGame", b =>
                {
                    b.Navigation("Runs");
                });

            modelBuilder.Entity("SudokuDataAccess.Models.UserProfile", b =>
                {
                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
