using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagementAPI.Entities.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasOne(a => a.Restaurant)
                   .WithOne(r => r.Address)
                   .HasForeignKey<Restaurant>(x => x.AddressId)
                   .OnDelete(DeleteBehavior.ClientCascade);
            builder.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
