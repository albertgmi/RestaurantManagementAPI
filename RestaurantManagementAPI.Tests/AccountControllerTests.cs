using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using RestaurantManagementAPI.Entities;
using RestaurantManagementAPI.Models;
using RestaurantManagementAPI.Services.UserServiceFolder;
using RestaurantManagementAPI.Tests.Helpers;
using System.Net;

namespace RestaurantManagementAPI.Tests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        public AccountControllerTests(WebApplicationFactory<Program> factory)
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
                        
                        services.AddSingleton<IUserService>(_userServiceMock.Object);

                        services
                        .AddDbContext<RestaurantDbContext>(options => options.UseInMemoryDatabase("RestaurantDb"));
                    });
                });
            _client = _factory.CreateClient();
        }
        [Fact]
        public async Task Register_ForValidUserParameters_ReturnsOkStatusCode()
        {
            // arrange
            var registeredUser = new RegisterUserDto()
            {
                Email = "test@test.com",
                Password = "password",
                ConfirmPassword = "password",
            };
            var httpContent = registeredUser.ToJsonHttpContent();
            // act
            var response = await _client.PostAsync("/api/account/register", httpContent);
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Register_ForInValidUserParameters_ReturnsBadRequestStatusCode()
        {
            // arrange
            var registeredUser = new RegisterUserDto()
            {
                Email = "testtest.com",
                Password = "password",
                ConfirmPassword = "1password2",
            };
            var httpContent = registeredUser.ToJsonHttpContent();
            // act
            var response = await _client.PostAsync("/api/account/register", httpContent);
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Login_ForRegisteredUser_ReturnsOkStatusCode()
        {
            // arrange
            _userServiceMock
                .Setup(x => x.GenerateJwt(It.IsAny<UserLoginDto>()))
                .Returns("jwtToken");
            var loginDto = new UserLoginDto()
            {
                Email = "test@test.com",
                Password = "password"
            };
            var httpContent = loginDto.ToJsonHttpContent();
            // act
            var response = await _client.PostAsync("/api/account/login", httpContent);
            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
