using FinalEcormer2023.Models;
using FinalEcormer2023.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalEcormer2023.Data {
	public class ApplicationDbContext : IdentityDbContext<IdentityUser> {
		//DB SET
		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
		/*public DbSet<Manufacturer> Manufacturers { get; set; }*/
		public DbSet<Order> Orders { get; set; }
		/*public DbSet<ProductManufacture> ProductManufactures { get; set; }*/
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);

            // Định dạng kiểu cột cho trường decimal 'Total' trong Order và 'Price' trong Product
            modelBuilder.Entity<Order>().Property(o => o.Total).HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18, 2)");

            // PRODUCT
            modelBuilder.Entity<Product>()
				.HasOne<Category>(c => c.category)
				.WithMany(p => p.products)
				.HasForeignKey(p => p.CategoryId).IsRequired(true);

			//CATEGORY
			modelBuilder.Entity<Category>().Property(f => f.Id).ValueGeneratedOnAdd();
			// PRODUCT-MANUFACTURE
			/*modelBuilder.Entity<ProductManufacture>().HasKey(pm => new { pm.ProductId, pm.ManufactureId });
			modelBuilder.Entity<ProductManufacture>().HasOne(pm => pm.Manufacturer).WithMany(pm => pm.productManufactures).HasForeignKey(p => p.ManufactureId);
			modelBuilder.Entity<ProductManufacture>().HasOne(p => p.Product).WithMany(pm => pm.productMannufactures).HasForeignKey(p => p.ProductId);*/
			// ORDER DETAIL
			modelBuilder.Entity<OrderDetail>()
				.HasOne(o => o.Order)
				.WithMany(o => o.orderDetails)
				.HasForeignKey(o => o.OrderId).IsRequired();

/*			modelBuilder.Entity<OrderDetail>()
				.HasOne(o => o.Product)
				.WithMany(o => o.).HasForeignKey(o => o.ProductId).IsRequired();*/
            modelBuilder.Seed();
		}

	}
}
