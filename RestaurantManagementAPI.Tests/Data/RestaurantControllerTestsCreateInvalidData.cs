using RestaurantManagementAPI.Models;
using System.Collections;

namespace RestaurantManagementAPI.Tests.Data
{
    public class RestaurantControllerTestsCreateInvalidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new CreateRestaurantDto()
                {
                    City = "Warszawa",
                    Street = "Nowoursynowska"
                }
            };
            yield return new object[]
            {
                new CreateRestaurantDto()
                {
                    Name = "Test",
                    City = "Siedlce"
                }
            };
            yield return new object[]
            {
                new CreateRestaurantDto()
                {
                    Name = "Basen",
                    Street = "Bond Street"
                }
            };
            yield return new object[]
            {
                new CreateRestaurantDto()
                {
                    PostalCode = "12345",
                    ContactEmail = "mio@mao.lalala"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
