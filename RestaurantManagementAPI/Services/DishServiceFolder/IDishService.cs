using RestaurantManagementAPI.Models;

namespace RestaurantManagementAPI.Services.DishServiceFolder
{
    public interface IDishService
    {
        Guid Create(string restaurantId, CreateDishDto dto);
        DishDto GetById(string restaurantId, string dishId);
        List<DishDto> GetAll(string restaurantId);
        void RemoveAll(string restaurantId);
        void RemoveById(string restaurantId, string dishId);
    }
}
