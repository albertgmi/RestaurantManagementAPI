using RestaurantManagementAPI.Models;
using System.Collections;

namespace RestaurantManagementAPI.Tests.Data
{
    public class RestaurantQueryValidatorTestsInvalidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new RestaurantQuery()
                {
                    PageNumber = 0,
                    PageSize = 10
                }
            };
            yield return new object[]
            {
                new RestaurantQuery()
                {
                    PageNumber = 1,
                    PageSize = 2
                }
            };
            yield return new object[]
            {
                new RestaurantQuery()
                {
                    PageNumber = 3,
                    PageSize = 3
                }
            };
            yield return new object[]
            {
                new RestaurantQuery()
                {
                    PageNumber = -1,
                    PageSize = -23
                }
            };
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
