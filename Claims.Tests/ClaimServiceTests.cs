
using Claims.Services;
using Xunit;

namespace Claims.Tests;

public class ClaimsServiceTests
{
    [Fact]
    public async Task AddItemAsync_WhenCoverDoesNotExist_ThrowsArgumentException()
    {
        // Arrange
        var fakeContext = new FakeClaimsContext();
        var service = new ClaimsService(fakeContext);

        var claim = new Claims.Models.Claim
        {
            CoverId = "non-existing-cover",
            DamageCost = 500m,
            Created = DateTime.Today
        };

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => service.AddItemAsync(claim));

        // Assert
        Assert.Equal("The specified cover does not exist.", exception.Message);
    }

    [Fact]
    public async Task AddItemAsync_WhenClaimIsValid_AddsClaimAndGeneratesId()
    {
        // Arrange
        var fakeContext = new FakeClaimsContext();

        var cover = new Claims.Models.Cover
        {
            Id = "cover-1",
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddMonths(6)
        };

        fakeContext.Covers.Add(cover);

        var service = new ClaimsService(fakeContext);

        var claim = new Claims.Models.Claim
        {
            CoverId = "cover-1",
            DamageCost = 500m,
            Created = DateTime.Today
        };

        // Act
        await service.AddItemAsync(claim);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(claim.Id));
        Assert.Contains(claim, fakeContext.Claims);
    }

    [Fact]
    public async Task AddItemAsync_WhenDamageCostExceedsLimit_ThrowsArgumentException()
    {
        // Arrange
        var fakeContext = new FakeClaimsContext();

        fakeContext.Covers.Add(new Claims.Models.Cover
        {
            Id = "cover-1",
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddMonths(6)
        });

        var service = new ClaimsService(fakeContext);

        var claim = new Claims.Models.Claim
        {
            CoverId = "cover-1",
            DamageCost = 100001m,
            Created = DateTime.Today
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.AddItemAsync(claim));
    }

    [Fact]
    public async Task AddItemAsync_WhenClaimDateIsOutsideCoverPeriod_ThrowsArgumentException()
    {
        // Arrange
        var fakeContext = new FakeClaimsContext();

        fakeContext.Covers.Add(new Claims.Models.Cover
        {
            Id = "cover-1",
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddMonths(6)
        });

        var service = new ClaimsService(fakeContext);

        var claim = new Claims.Models.Claim
        {
            CoverId = "cover-1",
            DamageCost = 500m,
            Created = DateTime.Today.AddDays(-1)
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.AddItemAsync(claim));
    }

    [Fact]
    public async Task DeleteItemAsync_WhenClaimExists_DeletesClaim()
    {
        // Arrange
        var fakeContext = new FakeClaimsContext();

        var claim = new Claims.Models.Claim
        {
            Id = "claim-1",
            CoverId = "cover-1",
            DamageCost = 500m,
            Created = DateTime.Today
        };

        fakeContext.Claims.Add(claim);

        var service = new ClaimsService(fakeContext);

        // Act
        await service.DeleteItemAsync("claim-1");

        // Assert
        Assert.DoesNotContain(claim, fakeContext.Claims);
    }

    [Fact]
    public async Task DeleteItemAsync_WhenClaimDoesNotExist_DoesNotThrow()
    {
        // Arrange
        var fakeContext = new FakeClaimsContext();
        var service = new ClaimsService(fakeContext);

        // Act & Assert
        var exception = await Record.ExceptionAsync(() => service.DeleteItemAsync("non-existing-claim"));

        Assert.Null(exception);
    }
}
