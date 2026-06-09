using Backend.DTOs;
using Backend.Models;

namespace Backend.Services
{
    public interface ITeamService
    {
        Task<Team> AddTeamAsync(TeamDto dto);
        Task<IEnumerable<Team>> GetAllTeamsAsync();
    }
}
