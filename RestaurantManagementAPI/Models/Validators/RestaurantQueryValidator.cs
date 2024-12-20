using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using RestaurantManagementAPI.Entities;

namespace RestaurantManagementAPI.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        private readonly int[] allowedPageSizes = new[] { 5, 10, 15 };
        private readonly string[] allowedSortByColumnNames = new[]
        {nameof(Restaurant.Name),nameof(Restaurant.Category), nameof(Restaurant.Description)};

        public RestaurantQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize)
                .Custom((value, context) =>
                {
                    if (!allowedPageSizes.Contains(value))
                    {
                        context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", allowedPageSizes)}]");
                    }
                });
            RuleFor(x => x.SortBy)
                .Must(value => value.IsNullOrEmpty() || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sort by is optional or must be in [{string.Join(", ", allowedSortByColumnNames)}]");
        }
    }
}
