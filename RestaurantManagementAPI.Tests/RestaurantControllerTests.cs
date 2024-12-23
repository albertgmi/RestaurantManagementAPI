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
        private readonly WebApplicationFactory<Program> _factory;
        public RestaurantControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory
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
                });
            _client = _factory.CreateClient();
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
        [Theory]
        [InlineData("8f1b8c13-3cb7-4e54-8f71-2c27b5a2c1df")]
        [InlineData("d5c7f6a8-814c-4972-872b-2cbdb591e6e4")]
        [InlineData("b231f687-5cd8-4b11-b8df-a3c66c1fd6e7")]
        [InlineData("aa03d6fa-7ab2-4c36-93fc-b2bcd4f6a8c5")]
        [InlineData("fc0d8b89-674a-4821-928e-3e2b62f5b814")]
        [InlineData("e8c64d2b-8312-4d61-bd9a-2e7f63a1c8e7")]
        public async Task Delete_WithInvalidId_ReturnsNotFoundStatusCode(string guid)
        {
            // act
            var result = await _client.DeleteAsync("/api/restaurant/" + Guid.Parse(guid));
            // assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Delete_ForRestaurantOwner_ReturnsNoContent()
        {
            // arrange
            var addedRestaurantId = AddCreatedRestaurantToDbContext(Guid.Parse("c1f7d8a4-bb6f-426d-98c2-5f34e7a62a8c"));

            // act
            var response = await _client.DeleteAsync("/api/restaurant/" + addedRestaurantId);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_ForNotAuthorizedUser_ReturnsForbiddenStatusCode()
        {
            // arrange
            var addedRestaurantId = AddCreatedRestaurantToDbContext(Guid.NewGuid());

            // act
            var response = await _client.DeleteAsync("/api/restaurant/" + addedRestaurantId);

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
        private Guid AddCreatedRestaurantToDbContext(Guid restaurantId)
        {
            var restaurant = new Restaurant()
            {
                CreatedById = restaurantId,
                Name = "Test"
            };

            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<RestaurantDbContext>();

            dbContext.Restaurants.Add(restaurant);
            dbContext.SaveChanges();
            return restaurant.Id;
        }
    }
}
