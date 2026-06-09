using Backend.DTOs;

namespace Backend.Services
{
    public interface IPollService
    {
        Task<string> VoteAsync(int userId, VoteDto dto);
        Task<string> OpenPollAsync();
        Task<string> ClosePollAsync();
        Task<bool> IsPollOpenAsync();
    }
}
