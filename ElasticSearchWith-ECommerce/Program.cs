using ElasticSearchWith_ECommerce.Extensions;
using ElasticSearchWith_ECommerce.Interfaces;
using ElasticSearchWith_ECommerce.Models;
using ElasticSearchWith_ECommerce.Repository;
using ElasticSearchWith_ECommerce.Services;

namespace ElasticSearchWith_ECommerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddElastic(builder.Configuration);
            builder.Services.AddScoped<IECommerceService, ECommerceService>();
            builder.Services.AddScoped<IECommerceRepository, ECommerceRepository>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}