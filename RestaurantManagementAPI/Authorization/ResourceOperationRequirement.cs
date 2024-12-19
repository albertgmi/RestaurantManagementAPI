using Microsoft.AspNetCore.Authorization;

namespace RestaurantManagementAPI.Authorization
{
    public enum ResourceOperation
    {
        Create,
        Update,
        Delete,
        Read
    }
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperationRequirement(ResourceOperation operation)
        {
            Operation = operation;
        }
        public ResourceOperation Operation { get; set; }
    }
}
