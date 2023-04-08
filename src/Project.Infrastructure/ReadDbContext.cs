using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Project.Application.Aggregates.Goods.Queries;
using Project.Application.Aggregates.Orders.Queries;
using Project.Application.Aggregates.Users.Queries;

namespace Project.Infrastructure
{
    public sealed class ReadDbContext : DbContext
    {
        public ReadDbContext(DbContextOptions<ReadDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
        }

        public DbSet<UserQueryResult> UserQueryResults { get; private set; }

        public DbSet<GoodQueryResult> GoodQueryResults { get; private set; }

        public DbSet<OrderQueryResult> OrderQueryResults { get; private set; }

        public DbSet<OrderItemQueryResult> OrderItemQueryResults { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("citext");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<OrderItemQueryResult>().HasKey(m => new {m.OrderId, m.GoodId});
            modelBuilder.Entity<OrderQueryResult>().Ignore(m => m.TotalPrice);

            base.OnModelCreating(modelBuilder);
        }
    }
}