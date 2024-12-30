using RestaurantManagementAPI.Models;
using System.Collections;

namespace RestaurantManagementAPI.Tests.Data
{
    public class RegisterUserDtoValidatorTestsInvalidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new RegisterUserDto()
                {
                    Email = "test2@test.com",
                    Password = "password12",
                    ConfirmPassword = "password12"
                }
            };
            yield return new object[]
            {
                new RegisterUserDto()
                {
                    Email = "test3@test.com",
                    Password = "password142",
                    ConfirmPassword = "password142"
                }
            };
            yield return new object[]
            {
                new RegisterUserDto()
                {
                    Email = "test4@test.com",
                    Password = "paassword12",
                    ConfirmPassword = "paassword12"
                }
            };
            yield return new object[]
            {
                new RegisterUserDto()
                {
                    Email = "test5@test.com",
                    Password = "1password12",
                    ConfirmPassword = "1password12"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
