using Backend.DTOs;

namespace Backend.Services
{
    public interface IResultService
    {
        Task<string> RevealResultAsync();
        Task<IEnumerable<ResultDto>> GetResultsAsync(bool isAdmin = false);
        Task<bool> IsResultVisibleAsync();
        Task<string> HideResultAsync();
    }
}
