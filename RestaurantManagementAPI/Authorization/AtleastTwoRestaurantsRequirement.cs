using Microsoft.AspNetCore.Authorization;

namespace RestaurantManagementAPI.Authorization
{
    public class AtleastTwoRestaurantsRequirement : IAuthorizationRequirement
    {
        public int MinimumRestaurantCreated { get; }
        public AtleastTwoRestaurantsRequirement(int minimumRestaurantCreated)
        {
            MinimumRestaurantCreated = minimumRestaurantCreated;
        }
    }
}
