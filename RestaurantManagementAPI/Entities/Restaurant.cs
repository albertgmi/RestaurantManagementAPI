namespace RestaurantManagementAPI.Entities
{
    public class Restaurant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasDelivery { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public Guid? CreatedById { get; set; }
        public User CreatedBy { get; set; }

        public Address Address { get; set; }
        public Guid AddressId { get; set; }
        public List<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
