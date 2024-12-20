using RestaurantManagementAPI.Models;

namespace RestaurantManagementAPI.Services.UserServiceFolder
{
    public interface IUserService
    {
        void RegisterUser(RegisterUserDto userDto);
        string GenerateJwt(UserLoginDto loginDto);
    }
}
