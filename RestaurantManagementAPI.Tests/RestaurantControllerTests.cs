using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using RestaurantManagementAPI.Entities;
using System.Net;
using Xunit.Abstractions;

namespace RestaurantManagementAPI.Tests
{
    public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public RestaurantControllerTests(WebApplicationFactory<Program> factory)
        {
             _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = 
                            services.SingleOrDefault(service => 
                                    service.ServiceType == typeof(DbContextOptions<RestaurantDbContext>));

                        services.Remove(dbContextOptions);

                        services
                        .AddDbContext<RestaurantDbContext>(options => options.UseInMemoryDatabase("RestaurantDb"));
                    });
                })
                .CreateClient();
        }

        [Theory]
        [InlineData("PageNumber=1&PageSize=5")]
        [InlineData("PageNumber=3&PageSize=10")]
        [InlineData("PageNumber=2&PageSize=15")]
        [InlineData("PageNumber=4&PageSize=5")]
        public async Task GetAll_WithQueryParameters_ReturnsOkStatusCode(string searchQuery)
        {
            // act
            var result = await _client.GetAsync("api/restaurant?" + searchQuery);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Theory]
        [InlineData("PageNumber=2&PageSize=4")]
        [InlineData("PageNumber=234&PageSize=18")]
        [InlineData("PageNumber=1&PageSize=16")]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetAll_WithQueryParameters_ReturnsBadRequestStatusCode(string searchQuery)
        {
            // act
            var result = await _client.GetAsync("api/restaurant?" + searchQuery);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
