using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using DriveSec.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.ConfigureServices((context, services) =>
        {
            string con = context.Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DriveSecContext>(options => options.UseNpgsql(con));

            services.AddControllersWithViews();
        })
    .Configure((context, app) =>
    {
        if (!context.HostingEnvironment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    })
    .ConfigureKestrel(options =>
    {
        options.Limits.MaxRequestBodySize = null; // or a given limit
    });
    });
}