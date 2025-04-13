using Microsoft.EntityFrameworkCore;

namespace OrderApi
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 定义 Order 与 OrderDetails 之间的关系
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne()
                .HasForeignKey(od => od.OrderId);
            modelBuilder.Entity<OrderDetails>()
            .Property(p => p.TotalAmount)
            .HasDefaultValue(0);  // 设置默认值（如果适用）
        }
    }
}
