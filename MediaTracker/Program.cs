using FilmTracker.Dtos;
using MediaTracker.Data;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

// --- LAB 1 SETUP: HttpClient for TMDB ---
// Paste your TMDB API Read Access Token (v4) here
const string tmdbBearerToken = "YOUR_TMDB_API_READ_ACCESS_TOKEN_HERE";

var client = new HttpClient
{
    BaseAddress = new Uri("https://api.themoviedb.org/3/")
};
client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", tmdbBearerToken);
client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/json"));

var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

// --- LAB 2 SETUP: DbContext for SQLite ---
using var db = new MediaTrackerDbContext();

Console.Write("Enter a movie title to search TMDB: ");
string query = Console.ReadLine() ?? "";

if (!string.IsNullOrWhiteSpace(query))
{
    try
    {
        // 1. Fetch from TMDB API (Lab 1)
        HttpResponseMessage response = client.GetAsync($"search/movie?query={Uri.EscapeDataString(query)}").Result;
        response.EnsureSuccessStatusCode();

        string responseBody = response.Content.ReadAsStringAsync().Result;
        var searchResult = JsonSerializer.Deserialize<TmdbSearchResponse>(responseBody, options);
        var firstMovie = searchResult?.Results.FirstOrDefault();

        if (firstMovie != null)
        {
            Console.WriteLine($"\nFound on TMDB: {firstMovie.Title}");
            Console.WriteLine($"Overview: {firstMovie.Overview}");

            // Parse release year safely
            int releaseYear = 0;
            if (DateTime.TryParse(firstMovie.ReleaseDate, out DateTime parsedDate))
            {
                releaseYear = parsedDate.Year;
            }

            // 2. Map DTO to DB Entity & Save (Lab 2)
            var newMedia = new MediaItem
            {
                TmdbId = firstMovie.Id,
                Title = firstMovie.Title,
                ReleaseYear = releaseYear,
                Overview = firstMovie.Overview,
                Type = MediaType.Movie,
                Status = WatchStatus.PlanToWatch,
                UserRating = 0,
                DateAdded = DateOnly.FromDateTime(DateTime.Now)
            };

            db.MediaItems.Add(newMedia);
            db.SaveChanges();

            Console.WriteLine($"\nSuccessfully saved '{newMedia.Title}' to your database!");
        }
        else
        {
            Console.WriteLine("No movies found matching that query.");
        }
    }
    catch (HttpRequestException e)
    {
        Console.WriteLine($"API Error: {e.Message}");
    }
}

// 3. Display Watchlist (Lab 2)
Console.WriteLine("\n--- Your Saved Watchlist ---");
var watchlist = db.MediaItems.ToList();

if (watchlist.Count == 0)
{
    Console.WriteLine("Your watchlist is currently empty.");
}
else
{
    foreach (var item in watchlist)
    {
        Console.WriteLine(item);
    }
}