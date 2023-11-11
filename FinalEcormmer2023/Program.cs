using FinalEcormer2023.Data;
using FinalEcormer2023.Interfaces;
using FinalEcormer2023.Seeding;
using FinalEcormer2023.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using FinalEcormer2023.Controllers;

namespace FinalEcormmer2023 {
	public class Program {
		public static void Main(string[] args) {
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<AccountController>();

            builder.Services.AddDbContext<ApplicationDbContext>(
				options => options.UseSqlServer(
					builder.Configuration.GetConnectionString("DbConnection")
					));

			builder.Services.AddIdentity<IdentityUser , IdentityRole>(
				options => {
					options.Password.RequiredLength = 8;
					options.Password.RequireLowercase = false;
					options.Password.RequireUppercase = false;
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequireDigit = false;

				}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

			builder.Services.AddTransient<IEmailSender, EmailSender>();
			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();

            //session
            builder.Services.AddDistributedMemoryCache();
			builder.Services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromSeconds(1800);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});

			builder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment()) {
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles(); 
			app.UseRouting();
			app.UseSession();
			app.UseAuthentication();;
			app.UseAuthorization();
			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			app.Run();
		}
	}
}