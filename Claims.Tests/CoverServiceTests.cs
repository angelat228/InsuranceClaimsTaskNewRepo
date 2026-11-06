
using Claims.Models;
using Claims.Services;
using Xunit;

namespace Claims.Tests;

public class CoversServiceTests
{
    [Fact]
    public void ComputePremium_WhenInsuranceIs30Days_ReturnsBaseRateWithTypeMarkup()
    {
        // Arrange
        var service = new CoversService(null!);

        // Act
        var premium = service.ComputePremium(DateTime.Today, DateTime.Today.AddDays(30), CoverType.Yacht);

        // Assert
        Assert.Equal(41250m, premium);
    }

    [Fact]
    public void ComputePremium_WhenInsuranceIs180Days_AppliesSecondPeriodDiscount()
    {
        var service = new CoversService(null!);

        var premium = service.ComputePremium(DateTime.Today, DateTime.Today.AddDays(180), CoverType.Yacht);

        // First 30 days: 30 × 1375
        // Next 150 days: 150 × (1375 × 0.95)
        Assert.Equal(237187.50m, premium);
    }

    [Fact]
    public void ComputePremium_WhenInsuranceIs365Days_AppliesThirdPeriodDiscount()
    {
        var service = new CoversService(null!);

        var premium = service.ComputePremium(DateTime.Today, DateTime.Today.AddDays(365), CoverType.Yacht);

        // First 30 days: 30 × 1375
        // Next 150 days: 150 × (1375 × 0.95)
        // Remaining 185 days: 185 × (1375 × 0.97)
        Assert.Equal(483931.25m, premium);
    }

    [Theory]
    [InlineData(CoverType.PassengerShip, 1.20)]
    [InlineData(CoverType.Tanker, 1.50)]
    [InlineData(CoverType.ContainerShip, 1.30)]
    [InlineData(CoverType.BulkCarrier, 1.30)]
    public void ComputePremium_WhenInsuranceIs30Days_UsesCorrectTypeMarkup(CoverType coverType, decimal multiplier)
    {
        var service = new CoversService(null!);

        var premium = service.ComputePremium(DateTime.Today, DateTime.Today.AddDays(30), coverType);

        Assert.Equal(30 * 1250m * multiplier, premium);
    }

    [Theory]
    [InlineData(CoverType.PassengerShip, 0.02, 0.01)]
    [InlineData(CoverType.Tanker, 0.02, 0.01)]
    [InlineData(CoverType.ContainerShip, 0.02, 0.01)]
    [InlineData(CoverType.BulkCarrier, 0.02, 0.01)]
    public void ComputePremium_WhenInsuranceIs365Days_AppliesCorrectDiscounts(CoverType coverType, decimal secondPeriodDiscount, decimal remainingPeriodDiscount)
    {
        var service = new CoversService(null!);

        var dailyRate = 1250m * (coverType switch
        {
            CoverType.PassengerShip => 1.20m,
            CoverType.Tanker => 1.50m,
            _ => 1.30m
        });

        var expected = 30 * dailyRate + 150 * dailyRate * (1m - secondPeriodDiscount) + 185 * dailyRate * (1m - remainingPeriodDiscount);

        var premium = service.ComputePremium(DateTime.Today, DateTime.Today.AddDays(365), coverType);

        Assert.Equal(expected, premium);
    }
}
