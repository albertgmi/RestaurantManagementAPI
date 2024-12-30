using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementAPI.Entities;
using RestaurantManagementAPI.Models;
using RestaurantManagementAPI.Models.Validators;
using RestaurantManagementAPI.Tests.Data;

namespace RestaurantManagementAPI.Tests.ValidatorTests
{
    public class RegisterUserDtoValidatorTests
    {
        private readonly RestaurantDbContext _dbContext;
        public RegisterUserDtoValidatorTests()
        {
            var builder = new DbContextOptionsBuilder<RestaurantDbContext>();
            builder.UseInMemoryDatabase("TestDb");

            _dbContext = new RestaurantDbContext(builder.Options);
            Seed();
        }
        private void Seed()
        {
            var testUsers = new List<User>()
            {
                new User()
                {
                    Email = "test2@test.com"
                },
                new User()
                {
                    Email = "test3@test.com"
                },
                new User()
                {
                    Email = "test4@test.com"
                },
                new User()
                {
                    Email = "test5@test.com"
                }
            };
            _dbContext.AddRange(testUsers);
            _dbContext.SaveChanges();
        }
        [Theory]
        [ClassData(typeof(RegisterUserDtoValidatorTestsData))]
        public void Validate_ForValidModel_ReturnsSuccess(RegisterUserDto model)
        {
            // arrange
            var validator = new RegisterUserDtoValidator(_dbContext);

            // act
            var result = validator.TestValidate(model);

            // assert
            result.ShouldNotHaveAnyValidationErrors();
        }
        [Theory]
        [ClassData(typeof(RegisterUserDtoValidatorTestsInvalidData))]
        public void Validate_ForTakenEmail_ReturnsFailure(RegisterUserDto model)
        {
            // arrange
            var validator = new RegisterUserDtoValidator(_dbContext);

            // act
            var result = validator.TestValidate(model);

            // assert
            result.ShouldHaveAnyValidationError();
        }
    }
}
