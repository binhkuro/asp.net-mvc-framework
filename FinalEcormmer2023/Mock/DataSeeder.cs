using FinalEcormer2023.Data;
using FinalEcormer2023.Models;
using Microsoft.AspNetCore.Identity;
using FinalEcormmer2023.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalEcormer2023.Seeding {
	public static class DataSeeder {
		public static void Seed(this ModelBuilder modelBuilder) {
			modelBuilder.Entity<Category>().HasData(new Category() { Id = 1, Name = "Iphone" });
			modelBuilder.Entity<Category>().HasData(new Category() { Id = 2, Name = "Ipad" });
			modelBuilder.Entity<Category>().HasData(new Category() { Id = 3, Name = "Mac" });
			modelBuilder.Entity<Category>().HasData(new Category() { Id = 4, Name = "Watch" });
			modelBuilder.Entity<Category>().HasData(new Category() { Id = 5, Name = "Phụ kiện" });
			modelBuilder.Entity<Product>().HasData(
				new Product() {
					Id = 1,
					Name = "iphone 15 Pro Max 1TB red",
					Description = "Ra mắt vào năm 2023",
					Url = "https://cdn.tgdd.vn/Products/Images/42/305659/iphone-15-pro-max-white-thumbnew-600x600.jpg",
					Price = 1000,
					CategoryId = 1,
					Color = "red"
				},
				new Product() {
					Id = 2,
					Name = "iphone 15 Pro Max 1TB white",
					Description = "Ra mắt vào năm 2023",
					Url = "https://cdn.tgdd.vn/Products/Images/42/305659/iphone-15-pro-max-white-thumbnew-600x600.jpg",
					Price = 3231,
					CategoryId = 1,
					Color = "white",
				},
				new Product() {
					Id = 3,
					Name = "iphone 15 Pro Max 1TB pink",
					Description = "Ra mắt vào năm 2023",
					Url = "https://shopdunk.com/images/thumbs/0022255_iphone-15-128gb_240.png",
					Price = 2313,
					CategoryId = 1,
					Color = "pink"
				},
				new Product() {
					Id = 4,
					Name = "Ipad Pro Max 1TB black",
					Description = "Ra mắt vào năm 2023",
					Url = "https://shopdunk.com/images/thumbs/0022263_iphone-15-pro-128gb_240.png",
					Price = 23838,
					CategoryId = 1,
					Color = "black",
				},
				new Product() {
					Id = 5,
					Name = "Ipad Pro Max 1TB green",
					Description = "Ra mắt vào năm 2023",
					Url = "https://shopdunk.com/images/thumbs/0000594_ipad-air-4_240.png",
					Price = 2133,
					CategoryId = 2,
					Color = "green",
				},
				new Product() {
					Id = 6,
					Name = "ipad-gen-9-102-inch-wifi-64gb",
					Description = "Ra mắt vào năm 2023",
					Url = "https://shopdunk.com/images/thumbs/0006205_ipad-gen-9-102-inch-wifi-64gb_240.png",
					Price = 5222,
					CategoryId = 2,
					Color = "black",
				}
				, new Product() {
					Id = 7,
					Name = "macbook-air-m1-2020-8gb-ram-256gb ",
					Description = "Ra mắt vào năm 2023",
					Url = "https://shopdunk.com/images/thumbs/0000723_macbook-air-m1-2020-8gb-ram-256gb-ssd_240.png",
					Price = 2994,
					CategoryId = 3,
					Color = "pink",
				}
				, new Product() {
					Id = 8,
					Name = "macbook-air-m2-2022-8gb-ram-256gbr",
					Description = "Ra mắt vào năm 2023",
					Url = "https://shopdunk.com/images/thumbs/0008502_macbook-air-m2-2022-8gb-ram-256gb-ssd_240.png",
					Price = 2000,
					CategoryId = 3,
					Color = "silver",
				}
				, new Product() {
					Id = 9,
					Name = "Macbook pro 13 black",
					Description = "Ra mắt vào năm 2023",
					Url = "https://shopdunk.com/images/thumbs/0008682_macbook-pro-13-inch-m2-10-core-8gb-ram-256gb-ssd_240.png",
					Price = 10000,
					CategoryId = 3,
					Color = "black",
				}
				, new Product() {
					Id = 10,
					Name = "Macbook pro 14 m2 black",
					Description = "Ra mắt vào năm 2023",
					Url = "https://shopdunk.com/images/thumbs/0011267_macbook-pro-14-inch-m2-pro-16-core-16gb-512gb_240.jpeg",
					Price = 12341,
					CategoryId = 4,
					Color = "black",
				}, new Product() {
					Id = 11,
					Name = "Watch se",
					Description = "Ra mắt vào năm 2023",
					Url = "https://shopdunk.com/images/thumbs/0022272_apple-watch-se-nhom-2022-gps_240.jpeg",
					Price = 20202,
					CategoryId = 4,
					Color = "grey",
				}
				, new Product() {
					Id = 12,
					Name = "Watch se",
					Description = "Ra mắt vào năm 2023",
					Url = "https://shopdunk.com/images/thumbs/0022275_apple-watch-se-2023-gps-sport-band-size-sm_240.jpeg",
					Price = 1999,
					CategoryId = 4,
					Color = "grey",
				}
				, new Product() {
					Id = 13,
					Name = "apple-watch-se-2023-gps-cellular",
					Description = "Ra mắt vào năm 2023",
					Url = "https://shopdunk.com/images/thumbs/0022328_apple-watch-se-2023-gps-cellular-sport-band-size-sm_240.png",
					Price = 2000,
					CategoryId = 4,
					Color = "grey",
				}
				, new Product() {
					Id = 14,
					Name = "sac-20w-usb-c-power-adapter",
					Description = "Ra mắt vào năm 2023",
					Url = "https://shopdunk.com/images/thumbs/0001395_sac-20w-usb-c-power-adapter_240.png",
					Price = 2000,
					CategoryId = 5,
					Color = "grey",
				}
				, new Product() {
					Id = 15,
					Name = "airpods-pro-2",
					Description = "Ra mắt vào năm 2023",
					Url = "https://shopdunk.com/images/thumbs/0000211_airpods-pro-2_240.png",
					Price = 2000,
					CategoryId = 5,
					Color = "grey",
				}
				, new Product() {
					Id = 16,
					Name = "tai-nghe-apple-airpods-3",
					Description = "Ra mắt vào năm 2023",
					Url = "https://shopdunk.com/images/thumbs/0006057_tai-nghe-apple-airpods-3-sac-co-day-lightning_240.jpeg",
					Price = 24197,
					CategoryId = 5,
					Color = "grey",
				}
			);

			modelBuilder.Entity<IdentityRole>().HasData(
				new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
				new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
			);

			var adminUserName = "admin";
            var adminPassword = "1234567890";
            var adminEmail = "caonguyenbinh12@gmail.com";

			var user = new IdentityUser
			{
				UserName = adminUserName,
				NormalizedUserName = adminUserName.ToUpper(),
				Email = adminEmail,
				NormalizedEmail = adminEmail.ToUpper(),
				EmailConfirmed = true,
				PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, adminPassword),
				SecurityStamp = string.Empty,
			};

			modelBuilder.Entity<IdentityUser>().HasData(user);

			var adminRoleId = "1";
			var adminUserRole = new IdentityUserRole<string>
			{
				UserId = user.Id,
				RoleId = adminRoleId
			};

			modelBuilder.Entity<IdentityUserRole<string>>().HasData(adminUserRole);
		}
	}
}
