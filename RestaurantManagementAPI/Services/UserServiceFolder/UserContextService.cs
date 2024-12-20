using System.Security.Claims;

namespace RestaurantManagementAPI.Services.UserServiceFolder
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _contextAccesor;
        public UserContextService(IHttpContextAccessor contextAccessor)
        {
            _contextAccesor = contextAccessor;
        }
        public ClaimsPrincipal User => _contextAccesor.HttpContext?.User;
        public Guid? GetUserId => User is null ? null : Guid.Parse(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }
}
