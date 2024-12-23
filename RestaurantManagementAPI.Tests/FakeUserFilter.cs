using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace RestaurantManagementAPI.Tests
{
    public class FakeUserFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            claimsPrincipal.AddIdentity(new ClaimsIdentity(
                new[]
                {
                new Claim(ClaimTypes.NameIdentifier, "c1f7d8a4-bb6f-426d-98c2-5f34e7a62a8c"),
                new Claim(ClaimTypes.Role, "Admin")
                }));
            context.HttpContext.User = claimsPrincipal;
            await next();
        }
    }
}
