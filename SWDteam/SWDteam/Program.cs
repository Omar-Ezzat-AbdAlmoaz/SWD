using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SWDteam.Data;
using Microsoft.AspNetCore.Identity;
using SWDteam.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(
    options => { options.UseSqlServer(builder.Configuration.GetConnectionString("SWDConnection")); }
    );

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()  
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddSession(
   options =>
   {
       options.IdleTimeout = TimeSpan.FromMinutes(30);
       options.Cookie.HttpOnly = true;
       options.Cookie.IsEssential = true;
   }
    );



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.UseSession();
app.Run();
