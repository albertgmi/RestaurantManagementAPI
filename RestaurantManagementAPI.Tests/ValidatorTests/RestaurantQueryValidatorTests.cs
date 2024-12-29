using FluentValidation.TestHelper;
using Microsoft.IdentityModel.Tokens;
using RestaurantManagementAPI.Models;
using RestaurantManagementAPI.Models.Validators;
using RestaurantManagementAPI.Tests.Data;

namespace RestaurantManagementAPI.Tests.ValidatorTests
{
    public class RestaurantQueryValidatorTests
    {
        [Theory]
        [ClassData(typeof(RestaurantQueryValidatorTestsData))]
        public void CreateRestaurant_ForValidQuery_ReturnsSucces(RestaurantQuery query)
        {
            // arrange
            var validator = new RestaurantQueryValidator();

            // act
            var result = validator.TestValidate(query);

            // assert
            result.ShouldNotHaveAnyValidationErrors();
        }
        [Theory]
        [ClassData(typeof(RestaurantQueryValidatorTestsInvalidData))]
        public void CreateRestaurant_ForInvalidQuery_ReturnsFalse(RestaurantQuery query)
        {
            // arrange
            var validator = new RestaurantQueryValidator();

            // act
            var result = validator.TestValidate(query);

            // assert
            result.ShouldHaveAnyValidationError();
        }
    }
}
