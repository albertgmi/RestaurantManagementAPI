using NuGet.Frameworks;
using RestaurantManagementAPI.Models;
using System.Collections;

namespace RestaurantManagementAPI.Tests.Data
{
    public class RestaurantQueryValidatorTestsData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new RestaurantQuery()
                {
                    PageNumber = 1,
                    PageSize = 10
                }
            };
            yield return new object[]
            {
                new RestaurantQuery()
                {
                    PageNumber = 2,
                    PageSize = 5
                }
            };
            yield return new object[]
            {
                new RestaurantQuery()
                {
                    PageNumber = 3,
                    PageSize = 15
                }
            };
            yield return new object[]
            {
                new RestaurantQuery()
                {
                    PageNumber = 5,
                    PageSize = 5
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
