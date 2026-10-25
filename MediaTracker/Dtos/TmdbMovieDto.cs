using System.Text.Json.Serialization;

namespace FilmTracker.Dtos
{
    public class TmdbSearchResponse
    {
        [JsonPropertyName("results")]
        public List<TmdbMovieDto> Results { get; set; } = new();
    }

    public class TmdbMovieDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("overview")]
        public string Overview { get; set; } = string.Empty;

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; } = string.Empty;
    }
}