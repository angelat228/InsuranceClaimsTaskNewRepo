
using Claims.Models;
using Claims.Services;
using Xunit;

namespace Claims.Tests;

public class ClaimValidatorTests
{
    [Fact]
    public void ValidateDamageCost_WhenCostExceeds100000_ThrowsArgumentException()
    {
        // Arrange
        var claim = new Claim
        {
            DamageCost = 100001m
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => ClaimValidator.ValidateDamageCost(claim));

        Assert.Equal("Claim damage cost cannot exceed 100,000.", exception.Message);
    }

    [Fact]
    public void ValidateCoverPeriod_WhenClaimDateIsBeforeCoverStarts_ThrowsArgumentException()
    {
        // Arrange
        var cover = new Cover
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddMonths(6),
            Type = CoverType.Yacht
        };

        var claim = new Claim
        {
            CoverId = "cover-1",
            Created = DateTime.Today.AddDays(-1),
            DamageCost = 500m
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => ClaimValidator.ValidateCoverPeriod(claim, cover));

        Assert.Equal("Claim creation date must fall within the cover period.", exception.Message);
    }

    [Fact]
    public void ValidateCoverPeriod_WhenClaimDateIsAfterCoverEnds_ThrowsArgumentException()
    {
        // Arrange
        var cover = new Cover
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddMonths(6),
            Type = CoverType.Yacht
        };

        var claim = new Claim
        {
            CoverId = "cover-1",
            Created = cover.EndDate.AddDays(1),
            DamageCost = 500m
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => ClaimValidator.ValidateCoverPeriod(claim, cover));

        Assert.Equal("Claim creation date must fall within the cover period.", exception.Message);
    }

    [Fact]
    public void ValidateCoverPeriod_WhenClaimDateIsWithinCoverPeriod_DoesNotThrow()
    {
        // Arrange
        var cover = new Cover
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddMonths(6),
            Type = CoverType.Yacht
        };

        var claim = new Claim
        {
            CoverId = "cover-1",
            Created = DateTime.Today.AddMonths(3),
            DamageCost = 500m
        };

        // Act & Assert
        var exception = Record.Exception(() => ClaimValidator.ValidateCoverPeriod(claim, cover));

        Assert.Null(exception);
    }

    [Fact]
    public void ValidateDamageCost_WhenCostIsExactly100000_DoesNotThrow()
    {
        // Arrange
        var claim = new Claim
        {
            DamageCost = 100000m
        };

        // Act & Assert
        var exception = Record.Exception(() => ClaimValidator.ValidateDamageCost(claim));

        Assert.Null(exception);
    }
}
