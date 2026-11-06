
using Claims.Data;
using Claims.Models;
namespace Claims.Services;

/// <summary>
/// Implements business operations for managing insurance claims.
/// </summary>

public class ClaimsService : IClaimsService
{
    private readonly IClaimsContext _claimsContext;

    public ClaimsService(IClaimsContext claimsContext)
    {
        _claimsContext = claimsContext;
    }

    public async Task<IEnumerable<Claim>> GetClaimsAsync()
    {
        return await _claimsContext.GetClaimsAsync();
    }

    public async Task<Claim?> GetClaimAsync(string id)
    {
        return await _claimsContext.GetClaimAsync(id);
    }

    public async Task AddItemAsync(Claim claim)
    {
        ClaimValidator.ValidateDamageCost(claim);

        var cover = await _claimsContext.GetCoverAsync(claim.CoverId);

        if (cover is null)
        {
            throw new ArgumentException(
                "The specified cover does not exist.");
        }

        ClaimValidator.ValidateCoverPeriod(claim, cover);

        claim.Id = Guid.NewGuid().ToString();

        await _claimsContext.AddItemAsync(claim);
    }
    public async Task DeleteItemAsync(string id)
    {
        await _claimsContext.DeleteItemAsync(id);
    }
}
