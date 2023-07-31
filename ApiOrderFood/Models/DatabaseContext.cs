using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ApiOrderFood.Models
{
    public partial class DatabaseContext : DbContext
    {

        public DatabaseContext()
        {
        }

        private readonly IConfiguration _configuration;
        public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:DefaultConnection"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("Id");
                entity.ToTable("mst_menu_items");
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(50);
                entity.Property(e => e.Price).HasMaxLength(50);
                entity.Property(e => e.Status).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("Id");
                entity.ToTable("mst_users");
                entity.Property(e => e.Username).HasMaxLength(50);
                entity.Property(e => e.Password).HasMaxLength(50);
                entity.Property(e => e.Role).HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("Id");
                entity.ToTable("tr_orders");
                entity.Property(e => e.TotalAmount).HasMaxLength(50);
                entity.Property(e => e.CreatedDate).HasMaxLength(50);
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.UpdatedBy).HasMaxLength(50);
                entity.Property(e => e.UpdatedDate).HasMaxLength(50);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.OrderItemId).HasName("Id");
                entity.ToTable("tr_order_items");
                entity.Property(e => e.OrderId).HasMaxLength(50);
                entity.Property(e => e.MenuItemId).HasMaxLength(50);
                entity.Property(e => e.Quantity).HasMaxLength(50);
                entity.Property(e => e.CreatedDate).HasMaxLength(50);
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.UpdatedBy).HasMaxLength(50);
                entity.Property(e => e.UpdatedDate).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
