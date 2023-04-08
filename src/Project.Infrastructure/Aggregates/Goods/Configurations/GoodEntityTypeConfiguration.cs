using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Aggregates.Goods;
using Project.Domain.Aggregates.Goods.ValueObjects;
using Project.Domain.ValueObjects;

namespace Project.Infrastructure.Aggregates.Goods.Configurations
{
    public class GoodEntityTypeConfiguration : BaseEntityTypeConfiguration<Good>
    {
        public override void Configure(EntityTypeBuilder<Good> builder)
        {
            builder.ToTable(nameof(WriteDbContext.Goods));

            builder.HasKey(i => i.Id);
            builder.HasIndex(i => i.Name).IsUnique();

            builder.Property(i => i.Id)
                .HasConversion(i => i.Value, i => GoodId.Create(i));

            builder.Property(i => i.Description)
                .HasConversion(i => i.Value, i => Description.Create(i));

            builder.Property(i => i.Name)
                .IsRequired()
                .HasColumnType("citext")
                .HasConversion(i => i.Value, i => GoodName.Create(i));

            builder.Property(i => i.Price)
                .IsRequired()
                .HasConversion(i => i.Value, i => GoodPrice.Create(i));

            builder.Property(i => i.Discount)
                .IsRequired()
                .HasConversion(i => i.Value, i => GoodDiscount.Create(i));

            builder.Property(i => i.IsFragile)
                .IsRequired()
                .HasConversion(i => i.Value, i => GoodIsFragile.Create(i));

            base.Configure(builder);
        }
    }
}