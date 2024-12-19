using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagementAPI.Entities.Configurations
{
    public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.HasMany(r => r.Dishes)
                .WithOne(d => d.Restaurant)
                .HasForeignKey(dfk => dfk.RestaurantId)
                .OnDelete(DeleteBehavior.ClientCascade);
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(25);
        }
    }
}
