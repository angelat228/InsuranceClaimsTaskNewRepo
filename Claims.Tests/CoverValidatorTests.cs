
using Claims.Models;
using Claims.Services;
using Xunit;

namespace Claims.Tests;

public class CoverValidatorTests
{
    [Fact]
    public void Validate_WhenStartDateIsInThePast_ThrowsArgumentException()
    {
        // Arrange
        var cover = new Cover
        {
            StartDate = DateTime.Today.AddDays(-1),
            EndDate = DateTime.Today.AddMonths(6),
            Type = CoverType.Yacht
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => CoverValidator.Validate(cover));

        Assert.Equal("Cover start date cannot be in the past.", exception.Message);
    }

    [Fact]
    public void Validate_WhenInsurancePeriodExceedsOneYear_ThrowsArgumentException()
    {
        // Arrange
        var cover = new Cover
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddYears(1).AddDays(1),
            Type = CoverType.Yacht
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => CoverValidator.Validate(cover));

        Assert.Equal("The insurance period cannot exceed one year.", exception.Message);
    }
}
