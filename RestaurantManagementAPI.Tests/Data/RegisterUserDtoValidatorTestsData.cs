using RestaurantManagementAPI.Models;
using System.Collections;

namespace RestaurantManagementAPI.Tests.Data
{
    public class RegisterUserDtoValidatorTestsData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new RegisterUserDto()
                {
                    Email = "test@test.pl",
                    Password = "password12",
                    ConfirmPassword = "password12"
                }
            };
            yield return new object[]
            {
                new RegisterUserDto()
                {
                    Email = "testemek@test.pl",
                    Password = "password142",
                    ConfirmPassword = "password142"
                }
            };
            yield return new object[]
            {
                new RegisterUserDto()
                {
                    Email = "registeremail@test.pl",
                    Password = "paassword12",
                    ConfirmPassword = "paassword12"
                }
            };
            yield return new object[]
            {
                new RegisterUserDto()
                {
                    Email = "validemail@test.pl",
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
