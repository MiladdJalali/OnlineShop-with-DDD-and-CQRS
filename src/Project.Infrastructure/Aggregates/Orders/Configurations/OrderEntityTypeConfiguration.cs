using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Aggregates.Goods;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Project.Domain.Aggregates.Orders;
using Project.Domain.Aggregates.Orders.Enums;
using Project.Domain.Aggregates.Orders.ValueObjects;
using Project.Domain.ValueObjects;

namespace Project.Infrastructure.Aggregates.Orders.Configurations
{
    public class OrderEntityTypeConfiguration : BaseEntityTypeConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(nameof(WriteDbContext.Orders));

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasConversion(i => i.Value, i => OrderId.Create(i));

            builder.Property(i => i.Description)
                .HasConversion(i => i.Value, i => Description.Create(i));

            builder.Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (OrderStatus) Enum.Parse(typeof(OrderStatus), v));

            builder.OwnsMany(i => i.OrderItems, j =>
            {
                j.ToTable("OrderItems");
                j.HasKey("OrderId", nameof(OrderItem.GoodId));
                j.Property(k => k.GoodId).HasConversion(i => i.Value, i => GoodId.Create(i));
                j.HasOne<Good>().WithMany().HasForeignKey(i => i.GoodId).OnDelete(DeleteBehavior.Restrict);
                j.Property(k => k.Count).IsRequired();
                j.Property(k => k.OrderPostType).HasConversion(v => v.ToString(),
                    v => (OrderPostType) Enum.Parse(typeof(OrderPostType), v));
            });

            base.Configure(builder);
        }
    }
}