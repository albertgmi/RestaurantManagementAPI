using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace RestaurantManagementAPI.Tests
{
    public class RestaurantControllerTests
    {
        [Fact]
        public async Task GetAll_WithQueryParameters_ReturnsOkStatusCode()
        {
            // arrange
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();

            // act
            var result = await client.GetAsync("api/restaurant?PageNumber=1&PageSize=5");

            // assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
