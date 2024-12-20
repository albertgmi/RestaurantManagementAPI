using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using Xunit.Abstractions;

namespace RestaurantManagementAPI.Tests
{
    public class RestaurantControllerTests
    {
        [Theory]
        [InlineData("PageNumber=1&PageSize=5", HttpStatusCode.OK)]
        [InlineData("PageNumber=3&PageSize=10", HttpStatusCode.OK)]
        [InlineData("PageNumber=2&PageSize=15", HttpStatusCode.OK)]
        [InlineData("PageNumber=4&PageSize=5", HttpStatusCode.OK)]
        [InlineData("PageNumber=2&PageSize=4", HttpStatusCode.BadRequest)]
        [InlineData("PageNumber=234&PageSize=18", HttpStatusCode.BadRequest)]
        [InlineData("PageNumber=1&PageSize=16", HttpStatusCode.BadRequest)]
        public async Task GetAll_WithQueryParameters_ReturnsOkStatusCode(string searchQuery, HttpStatusCode statusCode)
        {
            // arrange
            var factory = new WebApplicationFactory<Program>();
            var client = factory.CreateClient();

            // act
            var result = await client.GetAsync("api/restaurant?"+searchQuery);

            // assert
            result.StatusCode.Should().Be(statusCode);
        }
    }
}
