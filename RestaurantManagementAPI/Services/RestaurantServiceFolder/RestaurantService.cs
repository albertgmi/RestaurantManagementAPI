using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantManagementAPI.Authorization;
using RestaurantManagementAPI.Entities;
using RestaurantManagementAPI.Exceptions;
using RestaurantManagementAPI.Models;
using RestaurantManagementAPI.Services.UserServiceFolder;
using System.Linq.Expressions;

namespace RestaurantManagementAPI.Services.RestaurantServiceFolder
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper,
            ILogger<RestaurantService> logger, IAuthorizationService authorizationService,
            IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public RestaurantDto GetById(string id)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .FirstOrDefault(x => x.Id == Guid.Parse(id));
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
            return restaurantDto;
        }
        public PagedResult<RestaurantDto> GetAll(RestaurantQuery query)
        {
            var baseQuery = _dbContext
                .Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .Where(x => query.SearchPhrase == null
                            || x.Name.ToLower().Contains(query.SearchPhrase.ToLower())
                            || x.Description.ToLower().Contains(query.SearchPhrase.ToLower()));

            if (!query.SortBy.IsNullOrEmpty())
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    {nameof(Restaurant.Name), x=>x.Name },
                    {nameof(Restaurant.Description), x=>x.Description},
                    {nameof(Restaurant.Category), x=>x.Category }
                };

                var selectedColumn = columnsSelector[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var restaurants = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();
            var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);
            var result = new PagedResult<RestaurantDto>(restaurantsDtos, totalItemsCount, query.PageSize, query.PageNumber);
            return result;
        }

        public Guid Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);

            var userId = _userContextService.GetUserId;

            restaurant.CreatedById = userId;

            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }
        public void Delete(string id)
        {
            _logger.LogError($"Restaurant with id: {Guid.Parse(id)} DELETE action invoked");
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == Guid.Parse(id));

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var user = _userContextService.User;

            var authorizationResult = _authorizationService.AuthorizeAsync(user, restaurant,
                new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Authorization failed");
            }

            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();
        }
        public void Update(string id, UpdateRestaurantDto dto)
        {
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(x => x.Id == Guid.Parse(id));

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var user = _userContextService.User;

            var authorizationResult = _authorizationService.AuthorizeAsync(user, restaurant,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Authorization failed");
            }
            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;
            _dbContext.SaveChanges();
        }
    }
}
