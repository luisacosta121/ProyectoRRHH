using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProyectoRRHH.Context;
using ProyectoRRHH.Data;

namespace ProyectoRRHH
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<EmpresaDatabaseContext>(
options =>
options.UseSqlServer(builder.Configuration["ConnectionString:EmpresaDBConnection"]));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<DA_Logica>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                option =>
                {
                    option.LoginPath = "/Acceso/Index";
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    option.AccessDeniedPath = "/Home/Privacy";


                });

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

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Acceso}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
