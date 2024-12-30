using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementAPI.Entities;
using System.Net;

namespace RestaurantManagementAPI.Tests
{
    public class DishControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;
        public DishControllerTests(WebApplicationFactory<Program> factory)
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
        [Fact]
        public async Task GetAll_ForExistingRestauration_ReturnsOkStatusCode()
        {
            // arrange
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<RestaurantDbContext>();
            var restaurants = dbContext.Restaurants.Take(4);
            // act
            foreach (var restaurant in restaurants)
            {
                var result = await _client.GetAsync($"/api/restaurant/{restaurant.Id.ToString()}/dish");
                // assert
                result.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
        private Guid AddCreatedRestaurantToDbContext(Guid createdById)
        {
            var restaurant = new Restaurant()
            {
                CreatedById = createdById,
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
