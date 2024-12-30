using RestaurantManagementAPI.Entities;
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
                    PageSize = 10,
                    SortBy = nameof(Restaurant.HasDelivery)
                }
            };
            yield return new object[]
            {
                new RestaurantQuery()
                {
                    PageNumber = 1,
                    PageSize = 2,
                    SortBy = nameof(Restaurant.ContactEmail)
                }
            };
            yield return new object[]
            {
                new RestaurantQuery()
                {
                    PageNumber = 3,
                    PageSize = 3,
                    SortBy = nameof(Restaurant.ContactNumber)
                }
            };
            yield return new object[]
            {
                new RestaurantQuery()
                {
                    PageNumber = -1,
                    PageSize = -23,
                    SortBy = nameof(Restaurant.ContactNumber)
                }
            };
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
