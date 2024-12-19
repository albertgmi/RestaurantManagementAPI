using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagementAPI.Entities.Configurations
{
    public class DishConfiguration : IEntityTypeConfiguration<Dish>
    {
        public void Configure(EntityTypeBuilder<Dish> builder)
        {
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(25);
            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
