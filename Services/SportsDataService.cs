using Newtonsoft.Json;
using PlayerPerfAnalysis.Models;
using System.Net.Http.Headers;

namespace PlayerPerfAnalysis.Services
{
    public class SportsDataService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "ca121a8b-45ba-4fc1-8171-90ae3dc05baa";  // Your API key

        public SportsDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Helper method to add the Authorization header to every request
        private void AddAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", _apiKey);
        }

        public async Task<List<Team>> GetTeamsAsync()
        {
            string apiUrl = "https://api.balldontlie.io/v1/teams";

            try
            {
                AddAuthorizationHeader();  // Add the API key to request headers

                var response = await _httpClient.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to fetch teams. Status Code: {response.StatusCode}, Error: {errorResponse}");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<Team>>(jsonResponse);

                // Define a list of current NBA team names
                var currentNBATeamNames = new HashSet<string>
        {
            "Atlanta Hawks", "Boston Celtics", "Brooklyn Nets", "Charlotte Hornets", "Chicago Bulls",
            "Cleveland Cavaliers", "Dallas Mavericks", "Denver Nuggets", "Detroit Pistons", "Golden State Warriors",
            "Houston Rockets", "Indiana Pacers", "LA Clippers", "Los Angeles Lakers", "Memphis Grizzlies",
            "Miami Heat", "Milwaukee Bucks", "Minnesota Timberwolves", "New Orleans Pelicans", "New York Knicks",
            "Oklahoma City Thunder", "Orlando Magic", "Philadelphia 76ers", "Phoenix Suns", "Portland Trail Blazers",
            "Sacramento Kings", "San Antonio Spurs", "Toronto Raptors", "Utah Jazz", "Washington Wizards"
        };

                // Filter teams based on the list and remove duplicates by team ID
                var currentNBATeams = apiResponse?.Data
                    .Where(team => currentNBATeamNames.Contains(team.FullName))
                    .GroupBy(team => team.Id)  // Group by team ID
                    .Select(group => group.First())  // Select the first entry in each group
                    .ToList();

                return currentNBATeams ?? new List<Team>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw new Exception("Failed to fetch teams.", ex);
            }
        }

        public async Task<List<PlayerStats>> GetRosterAsync(int teamId)
        {
            string apiUrl = $"https://api.balldontlie.io/v1/players?team_ids[]={teamId}&per_page=15";

            try
            {
                AddAuthorizationHeader();  // Add API key

                var response = await _httpClient.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to fetch team roster. Status Code: {response.StatusCode}, Error: {errorResponse}");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonResponse); // Debugging: Log the API response

                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<PlayerStats>>(jsonResponse);

                // Filter out duplicates by player ID
                var uniquePlayers = apiResponse?.Data
                    .GroupBy(player => player.Id)
                    .Select(group => group.First())
                    .ToList();

                return uniquePlayers ?? new List<PlayerStats>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw new Exception("Failed to fetch team roster.", ex);
            }
        }

        public async Task<PlayerStats> GetPlayerStatsAsync(int playerId)
        {
            string apiUrl = $"https://api.balldontlie.io/v1/players/{playerId}";

            try
            {
                AddAuthorizationHeader();  // Add API key

                var response = await _httpClient.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to fetch player stats. Status Code: {response.StatusCode}, Error: {errorResponse}");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PlayerStats>(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw new Exception("Failed to fetch player stats.", ex);
            }
        }
    }

    public class ApiResponse<T>
    {
        public List<T> Data { get; set; } = new();
    }
}