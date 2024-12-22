using RestaurantManagementAPI.Models;
using System.Collections;

namespace RestaurantManagementAPI.Tests
{
    public class RestaurantControllerTestsCreateData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new CreateRestaurantDto()
                {
                    Name = "TestRestaurant",
                    City = "Warszawa",
                    Street = "Nowoursynowska"
                }
            };
            yield return new object[]
            {
                new CreateRestaurantDto()
                {
                    Name = "Test",
                    City = "Siedlce",
                    Street = "Florianska"
                }
            };
            yield return new object[]
            {
                new CreateRestaurantDto()
                {
                    Name = "Basen",
                    City = "Londyn",
                    Street = "Bond Street"
                }
            };
            yield return new object[]
            {
                new CreateRestaurantDto()
                {
                    Name = "Legia",
                    City = "Warszawa",
                    Street = "Łazienkowska"
                }
            };
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
