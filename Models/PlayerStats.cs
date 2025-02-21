using Newtonsoft.Json;

namespace PlayerPerfAnalysis.Models
{
    public class PlayerStats
    {
        public int Id { get; set; }  // Player ID

        [JsonProperty("first_name")]  // Maps to "first_name" in JSON
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty("last_name")]  // Maps to "last_name" in JSON
        public string LastName { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;  // G, F, C, etc.

        [JsonProperty("height")]  // Height (e.g., 6'3")
        public string Height { get; set; } = string.Empty;

        [JsonProperty("weight")]  // Weight in pounds (nullable)
        public int? Weight { get; set; }  // Changed to int?

        [JsonProperty("jersey_number")]  // Jersey number
        public string JerseyNumber { get; set; } = string.Empty;

        [JsonProperty("college")]  // College the player attended
        public string College { get; set; } = string.Empty;

        [JsonProperty("team")]  // Associated team information
        public Team Team { get; set; } = new Team();
    }

    public class Team
    {
        public int Id { get; set; }  // Team ID

        [JsonProperty("full_name")]  // Maps to "full_name" in JSON
        public string FullName { get; set; } = string.Empty;

        [JsonProperty("abbreviation")]  // Team abbreviation (e.g., "GSW")
        public string Abbreviation { get; set; } = string.Empty;

        [JsonProperty("city")]  // City (e.g., "Golden State")
        public string City { get; set; } = string.Empty;

        [JsonProperty("conference")]  // Conference (e.g., "West")
        public string Conference { get; set; } = string.Empty;

        [JsonProperty("division")]  // Division (e.g., "Pacific")
        public string Division { get; set; } = string.Empty;
    }
}