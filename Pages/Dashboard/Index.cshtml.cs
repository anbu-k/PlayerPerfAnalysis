using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlayerPerfAnalysis.Models;
using PlayerPerfAnalysis.Services;

namespace PlayerPerfAnalysis.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly SportsDataService _sportsDataService;

        public IndexModel(SportsDataService sportsDataService)
        {
            _sportsDataService = sportsDataService;
        }

        public List<Team> Teams { get; set; } = new();
        public List<PlayerStats> TeamRoster { get; set; } = new();
        public PlayerStats SelectedPlayer { get; set; }

        // Loads list of teams
        public async Task OnGetAsync()
        {
            Teams = await _sportsDataService.GetTeamsAsync();
        }

        // Handles displaying the roster when a team is selected
        public async Task<IActionResult> OnPostViewRosterAsync(int teamId)
        {
            TeamRoster = await _sportsDataService.GetRosterAsync(teamId);
            return Page();
        }

        // Handles displaying the player stats when a player is selected
        public async Task<IActionResult> OnPostViewPlayerStatsAsync(int playerId)
        {
            SelectedPlayer = await _sportsDataService.GetPlayerStatsAsync(playerId);
            return Page();
        }
    }
}
