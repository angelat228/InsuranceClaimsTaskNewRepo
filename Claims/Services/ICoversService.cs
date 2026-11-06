
using Claims.Models;

namespace Claims.Services;

/// <summary>
/// Defines operations for managing insurance covers.
/// </summary>
public interface ICoversService
{
    Task<IEnumerable<Cover>> GetCoversAsync();

    Task<Cover?> GetCoverAsync(string id);

    Task<Cover> CreateCoverAsync(Cover cover);

    Task DeleteCoverAsync(string id);

    decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType);
}
