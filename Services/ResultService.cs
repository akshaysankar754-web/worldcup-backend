using Backend.Data;
using Backend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class ResultService : IResultService
    {
        private readonly ApplicationDbContext _context;

        public ResultService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> RevealResultAsync()
        {
            var setting = await _context.Settings.FirstOrDefaultAsync();
            if (setting == null)
            {
                setting = new Models.Setting { ResultVisible = true };
                _context.Settings.Add(setting);
            }
            else
            {
                setting.ResultVisible = true;
            }
            
            await _context.SaveChangesAsync();
            return "Result revealed successfully.";
        }

        public async Task<string> HideResultAsync()
        {
            var setting = await _context.Settings.FirstOrDefaultAsync();
            if (setting != null)
            {
                setting.ResultVisible = false;
                await _context.SaveChangesAsync();
            }
            return "Result hidden successfully.";
        }

        public async Task<IEnumerable<ResultDto>> GetResultsAsync(bool isAdmin = false)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync();
            if (!isAdmin && (setting == null || !setting.ResultVisible))
            {
                throw new Exception("Results are not available yet.");
            }

            var results = await _context.Polls
                .Include(p => p.Team)
                .GroupBy(p => p.Team!.TeamName)
                .Select(g => new ResultDto
                {
                    TeamName = g.Key,
                    TotalVotes = g.Count()
                })
                .OrderByDescending(r => r.TotalVotes)
                .ToListAsync();

            return results;
        }

        public async Task<bool> IsResultVisibleAsync()
        {
            var setting = await _context.Settings.FirstOrDefaultAsync();
            return setting?.ResultVisible ?? false;
        }
    }
}
