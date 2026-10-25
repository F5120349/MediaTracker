using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MediaTracker.Data
{
    public class MediaItem
    {
        // Primary key for the database
        public int Id { get; set; }
        // Unique identifier from TMDb (The Movie Database)
        public int TmdbId { get; set; }

        // Core Info
        [Required]
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public string Genre { get; set; }
        public string Overview { get; set; }
        public int RuntimeMinutes { get; set; }


        // Using the enums here
        public MediaType Type { get; set; }
        public WatchStatus Status { get; set; }

        public int UserRating { get; set; } // Assuming a rating scale of 1-10
        public DateOnly DateAdded { get; set; }

        // Default constructor
        public MediaItem()
        {
            Title = string.Empty;
            Genre = string.Empty;
            Overview = string.Empty;
        }

        // Parameterised constructor
        public MediaItem(int tmdbId, string title, int releaseYear, string genre,
            string overview, int runtimeMinutes, MediaType type,
            WatchStatus status, int userRating, DateOnly dateAdded)
        {
            TmdbId = tmdbId;
            Title = title;
            ReleaseYear = releaseYear;
            Genre = genre;
            Overview = overview;
            RuntimeMinutes = runtimeMinutes;
            Type = type;
            Status = status;
            UserRating = userRating;
            DateAdded = dateAdded;
        }

        // Method to display media item details
        public override string ToString()
        {
            return $"{Id}: {Title} ({ReleaseYear}) - {Type} | {Genre} - Status: {Status} | Rating: {UserRating}/10 ({DateAdded.ToShortDateString()})";
        }
    } // End of class
}
