using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace MediaTracker.Data
{
    public class MediaTrackerDbContext : DbContext
    {
        public string DbPath { get; } // Path to the SQLite database file

        public MediaTrackerDbContext()
        {
            // Set the database file name (stored in the user's Documents folder)
            var folder = Environment.SpecialFolder.MyDocuments;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "Media_Tracker.db");
        }

        // Specify the database table
        public DbSet<MediaItem> MediaItems => Set<MediaItem>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            // Set the database filename (inc. path) for SQLite to use
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
