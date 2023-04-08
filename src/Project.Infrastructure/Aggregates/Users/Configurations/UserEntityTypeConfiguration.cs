using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Aggregates.Users;
using Project.Domain.Aggregates.Users.ValueObjects;
using Project.Domain.ValueObjects;

namespace Project.Infrastructure.Aggregates.Users.Configurations
{
    public class UserEntityTypeConfiguration : BaseEntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(WriteDbContext.Users));

            builder.HasKey(i => i.Id);
            builder.HasIndex(i => i.Username).IsUnique();

            builder.Property(i => i.Id)
                .HasConversion(i => i.Value, i => UserId.Create(i));

            builder.Property(i => i.Description)
                .HasConversion(i => i.Value, i => Description.Create(i));

            builder.Property(i => i.Username)
                .IsRequired()
                .HasColumnType("citext")
                .HasConversion(i => i.Value, i => UserUsername.Create(i));

            builder.Property(i => i.Password)
                .IsRequired()
                .HasConversion(i => i.Value, i => UserPassword.Create(i));

            builder.Property(i => i.Address)
                .IsRequired()
                .HasConversion(i => i.Value, i => UserAddress.Create(i));

            base.Configure(builder);
        }
    }
}