using RestaurantManagementAPI.Models;

namespace RestaurantManagementAPI.Services.RestaurantServiceFolder
{
    public interface IRestaurantService
    {
        Guid Create(CreateRestaurantDto dto);
        PagedResult<RestaurantDto> GetAll(RestaurantQuery query);
        RestaurantDto GetById(string id);
        void Delete(string id);
        void Update(string id, UpdateRestaurantDto dto);
    }
}
