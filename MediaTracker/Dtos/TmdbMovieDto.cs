using System.Text.Json.Serialization;

namespace MediaTracker.Dtos
{
    public class TmdbSearchResponse
    {
        [JsonPropertyName("results")]
        public List<TmdbSearchResultDto> Results { get; set; } = new();
    }

    public class TmdbSearchResultDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        // Movies use "title", TV shows use "name"
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("overview")]
        public string Overview { get; set; } = string.Empty;

        // Movies use "release_date", TV shows use "first_air_date"
        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("first_air_date")]
        public string? FirstAirDate { get; set; }

        // Display property that picks whichever title exists
        public string DisplayTitle => !string.IsNullOrEmpty(Title) ? Title : (Name ?? "Unknown Title");

        // Display property that picks whichever release date exists
        public string DisplayDate => !string.IsNullOrEmpty(ReleaseDate) ? ReleaseDate : (FirstAirDate ?? "");
    }
}