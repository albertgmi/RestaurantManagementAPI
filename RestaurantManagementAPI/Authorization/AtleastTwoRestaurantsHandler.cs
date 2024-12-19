using Microsoft.AspNetCore.Authorization;

namespace RestaurantManagementAPI.Authorization
{
    public class AtleastTwoRestaurantsHandler : AuthorizationHandler<AtleastTwoRestaurantsRequirement>
    {
        private readonly RestaurantDbContext _dbContext;
        public AtleastTwoRestaurantsHandler(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AtleastTwoRestaurantsRequirement requirement)
        {
            var userId = Guid.Parse(context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var usersRestaurants = _dbContext.Restaurants
                .Where(x => x.CreatedById == userId)
                .ToList();

            var usersRestaurantCount = usersRestaurants
                .Count();

            if (usersRestaurantCount >= requirement.MinimumRestaurantCreated)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
