using System.Security.Claims;

namespace RestaurantManagementAPI.Services.UserServiceFolder
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        Guid? GetUserId { get; }
    }
}
