using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain;

namespace Project.Infrastructure
{
    public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
        where T : Entity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property<Guid>("CreatorId");
            builder.Property<Guid?>("UpdaterId");
            builder.Property<DateTimeOffset>("Created");
            builder.Property<DateTimeOffset?>("Updated");
        }
    }
}