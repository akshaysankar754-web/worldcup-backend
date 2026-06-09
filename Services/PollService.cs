using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public class PollService : IPollService
    {
        private readonly ApplicationDbContext _context;

        public PollService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> VoteAsync(int userId, VoteDto dto)
        {
            var isPollOpen = await IsPollOpenAsync();
            if (!isPollOpen)
            {
                throw new Exception("The poll is currently closed. Please wait for the admin to open it.");
            }

            if (await _context.Polls.AnyAsync(p => p.UserId == userId))
            {
                throw new Exception("You have already submitted your vote.");
            }

            if (!await _context.Teams.AnyAsync(t => t.Id == dto.TeamId))
            {
                throw new Exception("Team not found.");
            }

            var poll = new Poll
            {
                UserId = userId,
                TeamId = dto.TeamId,
                VoteDate = DateTime.UtcNow
            };

            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();

            return "Vote submitted successfully.";
        }

        public async Task<string> OpenPollAsync()
        {
            var settings = await _context.Settings.FirstOrDefaultAsync();
            if (settings != null)
            {
                settings.PollOpen = true;
                await _context.SaveChangesAsync();
            }
            return "Poll opened successfully.";
        }

        public async Task<string> ClosePollAsync()
        {
            var settings = await _context.Settings.FirstOrDefaultAsync();
            if (settings != null)
            {
                settings.PollOpen = false;
                await _context.SaveChangesAsync();
            }
            return "Poll closed successfully.";
        }

        public async Task<bool> IsPollOpenAsync()
        {
            var settings = await _context.Settings.FirstOrDefaultAsync();
            return settings?.PollOpen ?? false;
        }
    }
}
