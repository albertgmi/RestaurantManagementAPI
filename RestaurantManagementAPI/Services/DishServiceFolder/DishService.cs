using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementAPI.Entities;
using RestaurantManagementAPI.Exceptions;
using RestaurantManagementAPI.Models;

namespace RestaurantManagementAPI.Services.DishServiceFolder
{
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<DishService> _logger;

        public DishService(RestaurantDbContext context, IMapper mapper, ILogger<DishService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public Guid Create(string restaurantId, CreateDishDto dto)
        {
            var restaurant = GetRestaurantAndDishesById(restaurantId);

            var dish = _mapper.Map<Dish>(dto);
            dish.RestaurantId = Guid.Parse(restaurantId);
            _context.Dishes.Add(dish);
            _context.SaveChanges();
            return dish.Id;
        }
        public DishDto GetById(string restaurantId, string dishId)
        {
            var restaurant = GetRestaurantAndDishesById(restaurantId);

            var dish = restaurant
                .Dishes
                .FirstOrDefault(x => x.Id == Guid.Parse(dishId));
            if (dish is null || dish.RestaurantId != restaurant.Id)
                throw new NotFoundException("Dish not found");
            var dishDto = _mapper.Map<DishDto>(dish);
            return dishDto;
        }
        public List<DishDto> GetAll(string restaurantId)
        {
            var restaurant = GetRestaurantAndDishesById(restaurantId);

            var dishes = restaurant.Dishes;
            var dishesDto = _mapper.Map<List<DishDto>>(dishes);
            return dishesDto;
        }
        public void RemoveAll(string restaurantId)
        {
            var restaurant = GetRestaurantAndDishesById(restaurantId);
            var dishes = restaurant.Dishes;
            _context.RemoveRange(dishes);
            _context.SaveChanges();
        }
        public void RemoveById(string restaurantId, string dishId)
        {
            var restaurant = GetRestaurantAndDishesById(restaurantId);
            var dish = _context
                .Dishes
                .FirstOrDefault(x => x.Id == Guid.Parse(dishId));
            if (dish == null)
                throw new NotFoundException("Dish not found");
            _context.Remove(dish);
            _context.SaveChanges();
        }
        private Restaurant GetRestaurantAndDishesById(string restaurantId)
        {
            var restaurant = _context
                .Restaurants
                .Include(x => x.Dishes)
                .FirstOrDefault(x => x.Id == Guid.Parse(restaurantId));
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");
            return restaurant;
        }
    }
}
