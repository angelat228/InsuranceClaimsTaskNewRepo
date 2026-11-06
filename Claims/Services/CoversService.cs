
using Claims.Data;
using Claims.Models;
using Microsoft.EntityFrameworkCore;

namespace Claims.Services;

/// <summary>
/// Implements business operations for managing insurance covers.
/// </summary>
public class CoversService : ICoversService
{
    private readonly ClaimsContext _claimsContext;

    public CoversService(ClaimsContext claimsContext)
    {
        _claimsContext = claimsContext;
    }

    public async Task<IEnumerable<Cover>> GetCoversAsync()
    {
        return await _claimsContext.Covers.ToListAsync();
    }

    public async Task<Cover?> GetCoverAsync(string id)
    {
        return await _claimsContext.Covers
            .SingleOrDefaultAsync(cover => cover.Id == id);
    }

    public async Task<Cover> CreateCoverAsync(Cover cover)
    {
        CoverValidator.Validate(cover);

        cover.Id = Guid.NewGuid().ToString();

        cover.Premium = ComputePremium(cover.StartDate, cover.EndDate, cover.Type);

        _claimsContext.Covers.Add(cover);
        await _claimsContext.SaveChangesAsync();

        return cover;
    }

    public async Task DeleteCoverAsync(string id)
    {
        var cover = await GetCoverAsync(id);

        if (cover is not null)
        {
            _claimsContext.Covers.Remove(cover);
            await _claimsContext.SaveChangesAsync();
        }
    }

    public decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        const decimal baseDayRate = 1250m;

        var typeMultiplier = coverType switch
        {
            CoverType.Yacht => 1.10m,
            CoverType.PassengerShip => 1.20m,
            CoverType.Tanker => 1.50m,
            _ => 1.30m
        };

        var dailyRate = baseDayRate * typeMultiplier;
        var insuranceDays = (endDate.Date - startDate.Date).Days;

        if (endDate.Date <= startDate.Date)
        {
            throw new ArgumentException(
                "End date must be later than start date.");
        }

        var firstPeriodDays = Math.Min(insuranceDays, 30);
        var secondPeriodDays = Math.Min(Math.Max(insuranceDays - 30, 0), 150);
        var remainingDays = Math.Max(insuranceDays - 180, 0);

        var firstPeriodPremium = firstPeriodDays * dailyRate;

        var secondPeriodDiscount = coverType == CoverType.Yacht
            ? 0.05m
            : 0.02m;

        var secondPeriodPremium = secondPeriodDays * dailyRate * (1m - secondPeriodDiscount);

        var remainingPeriodDiscount = coverType == CoverType.Yacht
            ? 0.03m
            : 0.01m;

        var remainingPeriodPremium = remainingDays * dailyRate * (1m - remainingPeriodDiscount);

        return firstPeriodPremium + secondPeriodPremium + remainingPeriodPremium;
    }
}
