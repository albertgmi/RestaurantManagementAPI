using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RestaurantManagementAPI.Authorization
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAgeRequirementHandler> _logger;
        public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger)
        {
            _logger = logger;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var dateOfBirth = DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth").Value);
            var userEmail = context.User.FindFirst(x => x.Type == ClaimTypes.Name).Value;
            _logger.LogInformation($"User: {userEmail} born: [{dateOfBirth}]");
            if (dateOfBirth.AddYears(requirement.MinimumAge) < DateTime.Today)
            {
                _logger.LogInformation("Authorization succeded");
                context.Succeed(requirement);
            }
            else
                _logger.LogInformation("Authorization failed");
            return Task.CompletedTask;
        }
    }
}
