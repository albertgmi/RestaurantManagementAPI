using RestaurantManagementAPI.Entities;

namespace RestaurantManagementAPI.Seeders
{
    public interface IRestaurantSeeder
    {
        void Seed(RestaurantDbContext dbContext);
    }
}
