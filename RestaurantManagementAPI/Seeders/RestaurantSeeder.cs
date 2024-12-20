using Bogus;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementAPI.Entities;

namespace RestaurantManagementAPI.Seeders
{
    public class RestaurantSeeder : IRestaurantSeeder
    {
        private readonly Faker faker = new Faker();

        public void Seed(RestaurantDbContext dbContext)
        {

            if (dbContext.Database.CanConnect())
            {
                if (dbContext.Database.IsRelational())
                {
                    var pendingMigrations = dbContext.Database.GetPendingMigrations();

                    if (pendingMigrations != null && pendingMigrations.Any())
                        dbContext.Database.Migrate();
                }
                if (dbContext.Dishes.Any())
                    return;
                if (!dbContext.Roles.Any())
                {
                    var roles = CreateRoles();
                    dbContext.Roles.AddRange(roles);
                    dbContext.SaveChanges();
                }
                var addresses = CreateAddresses();
                dbContext.Addresses.AddRange(addresses);
                dbContext.SaveChanges();

                var restaurants = CreateRestaurants(dbContext.Addresses.ToList());
                dbContext.Restaurants.AddRange(restaurants);
                dbContext.SaveChanges();

                var dishes = CreateDishes(dbContext.Restaurants.ToList());
                dbContext.Dishes.AddRange(dishes);
                dbContext.SaveChanges();
            }
        }

        public List<Address> CreateAddresses()
        {
            var addresses = new List<Address>();
            for (int i = 0; i < 100; i++)
            {
                addresses.Add(new Address
                {
                    City = faker.Address.City(),
                    Street = faker.Address.StreetName(),
                    PostalCode = faker.Address.ZipCode()
                });
            }
            return addresses;
        }
        public List<Restaurant> CreateRestaurants(List<Address> addresses)
        {
            var restaurants = new List<Restaurant>();
            List<string> restaurantCategories = new List<string>
            {
                "Italian", "Chinese", "Mexican", "Japanese", "French",
                "Indian", "American", "Thai", "Spanish", "Mediterranean",
                "Vegetarian", "Fast Food", "Steakhouse", "Seafood",
                "Pizzeria", "Cafe", "Grill", "Barbecue", "Desserts"
            };
            for (int i = 0; i < 100; i++)
            {
                var faker2 = new Faker();
                restaurants.Add(new Restaurant
                {
                    Name = faker.Commerce.Product(),
                    Description = faker.Commerce.ProductDescription(),
                    Category = faker.PickRandom(restaurantCategories),
                    HasDelivery = faker.Random.Bool(),
                    ContactEmail = faker2.Person.Email,
                    ContactNumber = faker2.Person.Phone,
                    AddressId = addresses[faker.Random.Int(0, addresses.Count - 1)].Id
                });
            }
            return restaurants;
        }
        public List<Dish> CreateDishes(List<Restaurant> restaurants)
        {
            var dishes = new List<Dish>();
            for (int i = 0; i < 100; i++)
            {
                dishes.Add(new Dish
                {
                    Name = faker.Commerce.Product(),
                    Description = faker.Commerce.ProductDescription(),
                    Price = Convert.ToDecimal(faker.Commerce.Price()),
                    RestaurantId = restaurants[faker.Random.Int(0, restaurants.Count - 1)].Id
                });
            }
            return dishes;
        }
        public List<Role> CreateRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Manager"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };
            return roles;
        }
    }
}
