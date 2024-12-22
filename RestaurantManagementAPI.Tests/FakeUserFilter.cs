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
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "Admin")
                }));
            context.HttpContext.User = claimsPrincipal;
            await next();
        }
    }
}
