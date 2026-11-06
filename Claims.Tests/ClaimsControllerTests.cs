using Claims.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Claims.Tests
{
    public class ClaimsControllerTests
    {
        [Fact]
        public async Task Get_Claims()
        {
            var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();

            var response = await client.GetAsync("/Claims");

            // Verify the HTTP status code
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Verify that the response contains JSON
            Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

            // Read the response body
            var claims = await response.Content.ReadFromJsonAsync<List<Claim>>();

            // Verify that a collection was returned
            Assert.NotNull(claims);
        }
    }
}