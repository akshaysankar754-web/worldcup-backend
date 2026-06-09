using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext _context;

        public TeamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Team> AddTeamAsync(TeamDto dto)
        {
            if (await _context.Teams.AnyAsync(t => t.TeamName == dto.TeamName))
            {
                throw new Exception("Team already exists");
            }

            var team = new Team
            {
                TeamName = dto.TeamName
            };

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return team;
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _context.Teams.ToListAsync();
        }
    }
}
