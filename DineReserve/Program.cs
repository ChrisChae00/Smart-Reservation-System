using DineReserve.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Register EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add session support
builder.Services.AddSession();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DineReserve.Data.AppDbContext>();

    context.Database.EnsureCreated(); // creates DB if not exists

    if (!context.Admins.Any())
    {
        context.Admins.Add(new DineReserve.Models.Admin
        {
            Username = "admin@restaurant.com",
            Password = "admin123"
        });

        context.SaveChanges();
    }
}