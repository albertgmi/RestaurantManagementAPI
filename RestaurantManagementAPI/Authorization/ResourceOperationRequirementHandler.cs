using Microsoft.AspNetCore.Authorization;
using RestaurantManagementAPI.Entities;
using System.Security.Claims;

namespace RestaurantManagementAPI.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Restaurant>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ResourceOperationRequirement requirement, Restaurant restaurant)
        {
            if (requirement.Operation == ResourceOperation.Read || requirement.Operation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }
            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (restaurant.CreatedById == Guid.Parse(userId))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
