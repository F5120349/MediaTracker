using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using MediaTracker.Data;
using MediaTracker.Dtos;

namespace MediaTracker
{
    internal class Program
    {
        // TMDB API Configuration
        private const string TmdbBearerToken = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI3ZjViOTgzNzU5MzEwNDY1MDg0MmNlYTZlYjRkYTMxYSIsIm5iZiI6MTc3MjQwOTMxOS4zMjUsInN1YiI6IjY5YTRkMWU3Njg2NzY2OGNmMGE3YTI5ZCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.E5K6rqWWc45V_ut06z5-BHrsuSbKRdsL6sHt12rsyaU";

        private static readonly HttpClient HttpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.themoviedb.org/3/")
        };

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        static void Main(string[] args)
        {
            // Configure HttpClient headers
            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TmdbBearerToken);
            HttpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("      FILM & TV MEDIA TRACKER           ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Search & Add Media (TMDB)");
                Console.WriteLine("2. View All Saved Media");
                Console.WriteLine("3. Update Watch Status & Rating");
                Console.WriteLine("4. Delete Media Item");
                Console.WriteLine("5. Exit");
                Console.WriteLine("========================================");
                Console.Write("Select an option (1-5): ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        SearchAndAddMedia();
                        break;
                    case "2":
                        ViewWatchlist();
                        break;
                    case "3":
                        UpdateMediaStatus();
                        break;
                    case "4":
                        DeleteMediaItem();
                        break;
                    case "5":
                        running = false;
                        Console.WriteLine("\nThank you for using Media Tracker. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("\nInvalid option. Press Enter to try again...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        // 1. SEARCH & ADD
        private static void SearchAndAddMedia()
        {
            Console.Clear();
            Console.WriteLine("=== Search TMDB ===");
            Console.WriteLine("1. Search Movie");
            Console.WriteLine("2. Search TV Show");
            Console.Write("Choice: ");
            string typeChoice = Console.ReadLine() ?? "";

            MediaType type = typeChoice == "2" ? MediaType.TVShow : MediaType.Movie;
            string endpoint = type == MediaType.Movie ? "search/movie" : "search/tv";

            Console.Write($"\nEnter {type} title to search: ");
            string query = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(query))
            {
                Console.WriteLine("Search query cannot be empty.");
                Pause();
                return;
            }

            try
            {
                HttpResponseMessage response = HttpClient.GetAsync($"{endpoint}?query={Uri.EscapeDataString(query)}").Result;
                response.EnsureSuccessStatusCode();

                string responseBody = response.Content.ReadAsStringAsync().Result;
                var searchResult = JsonSerializer.Deserialize<TmdbSearchResponse>(responseBody, JsonOptions);

                var results = searchResult?.Results.Take(5).ToList();

                if (results == null || results.Count == 0)
                {
                    Console.WriteLine("\nNo results found on TMDB.");
                    Pause();
                    return;
                }

                Console.WriteLine($"\n--- TMDB Search Results ({type}) ---");
                for (int i = 0; i < results.Count; i++)
                {
                    int year = 0;
                    if (DateTime.TryParse(results[i].DisplayDate, out DateTime parsedDate))
                    {
                        year = parsedDate.Year;
                    }
                    Console.WriteLine($"{i + 1}. {results[i].DisplayTitle} ({year})");
                }

                Console.Write("\nSelect number to save (0 to cancel): ");
                if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= results.Count)
                {
                    var selected = results[selection - 1];

                    int releaseYear = 0;
                    if (DateTime.TryParse(selected.DisplayDate, out DateTime parsedDate))
                    {
                        releaseYear = parsedDate.Year;
                    }

                    using var db = new MediaTrackerDbContext();

                    // Check if already in database
                    if (db.MediaItems.Any(m => m.TmdbId == selected.Id && m.Type == type))
                    {
                        Console.WriteLine($"\n'{selected.DisplayTitle}' is already in your database!");
                    }
                    else
                    {
                        var newItem = new MediaItem
                        {
                            TmdbId = selected.Id,
                            Title = selected.DisplayTitle,
                            ReleaseYear = releaseYear,
                            Overview = selected.Overview,
                            Type = type,
                            Status = WatchStatus.PlanToWatch,
                            UserRating = 0,
                            DateAdded = DateOnly.FromDateTime(DateTime.Now)
                        };

                        db.MediaItems.Add(newItem);
                        db.SaveChanges();

                        Console.WriteLine($"\nSuccessfully saved '{newItem.Title}' to your database!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError communicating with TMDB: {ex.Message}");
            }

            Pause();
        }

        // 2. VIEW ALL
        private static void ViewWatchlist()
        {
            Console.Clear();
            Console.WriteLine("=== Your Saved Media Library ===");

            using var db = new MediaTrackerDbContext();
            var list = db.MediaItems.OrderBy(m => m.Id).ToList();

            if (list.Count == 0)
            {
                Console.WriteLine("\nYour library is empty. Try searching and adding items!");
            }
            else
            {
                Console.WriteLine();
                foreach (var item in list)
                {
                    Console.WriteLine(item);
                }
            }

            Pause();
        }

        // 3. UPDATE STATUS & RATING
        private static void UpdateMediaStatus()
        {
            Console.Clear();
            Console.WriteLine("=== Update Watch Status / Rating ===");

            using var db = new MediaTrackerDbContext();
            var list = db.MediaItems.ToList();

            if (list.Count == 0)
            {
                Console.WriteLine("\nNo media items to update.");
                Pause();
                return;
            }

            foreach (var item in list)
            {
                Console.WriteLine(item);
            }

            Console.Write("\nEnter the ID of the item to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var item = db.MediaItems.FirstOrDefault(m => m.Id == id);
                if (item != null)
                {
                    Console.WriteLine($"\nUpdating: {item.Title}");
                    Console.WriteLine("Status Options: 0 = PlanToWatch, 1 = Watching, 2 = Completed, 3 = Dropped");
                    Console.Write("Enter new status number: ");

                    if (int.TryParse(Console.ReadLine(), out int statusNum) && Enum.IsDefined(typeof(WatchStatus), statusNum))
                    {
                        item.Status = (WatchStatus)statusNum;
                    }

                    Console.Write("Enter rating (1-10): ");
                    if (int.TryParse(Console.ReadLine(), out int rating) && rating >= 1 && rating <= 10)
                    {
                        item.UserRating = rating;
                    }

                    db.SaveChanges();
                    Console.WriteLine($"\nSuccessfully updated '{item.Title}'!");
                }
                else
                {
                    Console.WriteLine("\nItem with that ID was not found.");
                }
            }

            Pause();
        }

        // 4. DELETE ITEM
        private static void DeleteMediaItem()
        {
            Console.Clear();
            Console.WriteLine("=== Delete Media Item ===");

            using var db = new MediaTrackerDbContext();
            var list = db.MediaItems.ToList();

            if (list.Count == 0)
            {
                Console.WriteLine("\nNo media items to delete.");
                Pause();
                return;
            }

            foreach (var item in list)
            {
                Console.WriteLine(item);
            }

            Console.Write("\nEnter the ID of the item to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var item = db.MediaItems.FirstOrDefault(m => m.Id == id);
                if (item != null)
                {
                    db.MediaItems.Remove(item);
                    db.SaveChanges();
                    Console.WriteLine($"\nSuccessfully removed '{item.Title}' from your database.");
                }
                else
                {
                    Console.WriteLine("\nItem with that ID was not found.");
                }
            }

            Pause();
        }

        private static void Pause()
        {
            Console.WriteLine("\nPress Enter to return to the main menu...");
            Console.ReadLine();
        }
    }
}