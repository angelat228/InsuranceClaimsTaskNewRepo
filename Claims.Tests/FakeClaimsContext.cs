
using Claims.Data;
using Claims.Models;

namespace Claims.Tests;

public class FakeClaimsContext : IClaimsContext
{
    public List<Claim> Claims { get; } = new();

    public List<Cover> Covers { get; } = new();

    public Task<IEnumerable<Claim>> GetClaimsAsync()
    {
        return Task.FromResult<IEnumerable<Claim>>(Claims.ToList());
    }

    public Task<Claim?> GetClaimAsync(string id)
    {
        return Task.FromResult(Claims.SingleOrDefault(claim => claim.Id == id));
    }

    public Task<Cover?> GetCoverAsync(string id)
    {
        return Task.FromResult(Covers.SingleOrDefault(cover => cover.Id == id));
    }

    public Task AddItemAsync(Claim item)
    {
        Claims.Add(item);
        return Task.CompletedTask;
    }

    public Task DeleteItemAsync(string id)
    {
        var claim = Claims.SingleOrDefault(claim => claim.Id == id);

        if (claim is not null)
        {
            Claims.Remove(claim);
        }

        return Task.CompletedTask;
    }
}
