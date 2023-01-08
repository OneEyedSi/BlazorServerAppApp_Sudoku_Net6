using Microsoft.EntityFrameworkCore;
using SudokuDataAccess.Models;
using SudokuDataAccess.Models.Reference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuDataAccess
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        /// <summary>
        /// Applies model configurations from this assembly.
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <remarks>Applying model configurations from this assembly is a good way of seeding 
        /// reference tables with data, via EntityTypeBuilder<T>.HasData(IEnumerable<T>).  It 
        /// allows the seed data to be defined alongside the model class, rather than in a 
        /// separate "seed data" class.</remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        #region Reference Entities ****************************************************************

        public DbSet<Icon> Icons { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Position> Positions { get; set; }

        #endregion

        #region User Entities *********************************************************************

        public DbSet<UserProfile> UserProfiles { get; set; }

        #endregion

        #region Unplayed Game Entities ************************************************************

        public DbSet<GameBoard> GameBoards { get; set; }
        public DbSet<GameBoardValue> GameBoardValues { get; set; }

        #endregion

        #region User Game Entities ****************************************************************

        public DbSet<UserGame> UserGames { get; set; }
        public DbSet<GameRun> GameRuns { get; set; }
        public DbSet<GameRunOption> GameRunOptions { get; set; }
        public DbSet<GameRunValue> GameRunValues { get; set; }
        public DbSet<GameRunHistoryEntry> GameRunHistoryEntry { get; set; }

        #endregion
    }
}
