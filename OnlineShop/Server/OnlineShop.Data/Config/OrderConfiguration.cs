using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Data.Models.Enumerations;
using OnlineShop.Data.Models.OrderAggregate;

namespace OnlineShop.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {   
            builder.OwnsOne(x => x.ShipToAddress, a => {
                a.WithOwner();
            });

            builder.Navigation(a => a.ShipToAddress).IsRequired();

            builder.Property(s => s.Status).HasConversion(
                o => o.ToString(),
                o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o));

            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);

        }
    }
}
