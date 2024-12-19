namespace RestaurantManagementAPI.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
