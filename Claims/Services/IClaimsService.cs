using Claims.Models;

namespace Claims.Services;

/// <summary>
/// Defines operations for managing insurance claims.
/// </summary>
public interface IClaimsService
{
    Task<IEnumerable<Claim>> GetClaimsAsync();
    Task<Claim?> GetClaimAsync(string id);
    Task AddItemAsync(Claim claim);
    Task DeleteItemAsync(string id);
}