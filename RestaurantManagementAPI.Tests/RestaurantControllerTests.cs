using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementAPI.Entities;
using RestaurantManagementAPI.Models;
using RestaurantManagementAPI.Tests.Data;
using RestaurantManagementAPI.Tests.Helpers;
using System.Net;

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

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                        services.AddMvc(x => x.Filters.Add(new FakeUserFilter()));

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
        [Theory]
        [ClassData(typeof(RestaurantControllerTestsCreateData))]
        public async Task CreateRestaurant_WithValidModel_ReturnsCreatedStatusCode(CreateRestaurantDto query)
        {
            // arrange
            var httpContent = query.ToJsonHttpContent();
            // act
            var response = await _client.PostAsync("api/restaurant", httpContent);
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }
        [Theory]
        [ClassData(typeof(RestaurantControllerTestsCreateInvalidData))]
        public async Task CreateRestaurant_WithInvalidModel_ReturnsBadRequestStatusCode(CreateRestaurantDto query)
        {
            // arrange
            var httpContent = query.ToJsonHttpContent();
            // act
            var response = await _client.PostAsync("api/restaurant", httpContent);
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
