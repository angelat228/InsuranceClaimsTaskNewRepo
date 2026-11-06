
using Claims.Models;

namespace Claims.Services;

public static class ClaimValidator
{
    public static void ValidateDamageCost(Claim claim)
    {
        if (claim.DamageCost > 100000m)
        {
            throw new ArgumentException("Claim damage cost cannot exceed 100,000.");
        }
    }


    public static void ValidateCoverPeriod(Claim claim, Cover cover)
    {
        var claimDate = claim.Created.Date;
        var coverStartDate = cover.StartDate.ToLocalTime().Date;
        var coverEndDate = cover.EndDate.ToLocalTime().Date;

        if (claimDate < coverStartDate || claimDate > coverEndDate)
        {
            throw new ArgumentException("Claim creation date must fall within the cover period.");
        }
    }
}
