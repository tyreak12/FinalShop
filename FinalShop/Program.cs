using Microsoft.EntityFrameworkCore;
using FinalShop.Models;
using FinalShop.Data;
using FinalShop.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlossomBoutiqueContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BlossomBoutiqueContext")));

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityContext")));

builder.Services
    .AddDefaultIdentity<ApplicationUser>(opts =>
    {
        opts.Password.RequireDigit = true;
        opts.Password.RequiredLength = 6;
        opts.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>();

// 4. Your application services
builder.Services.AddScoped<IProductService, ProductService>();

// 5. Add MVC, Razor Pages, and Session support
builder.Services.AddControllersWithViews();    // includes API controllers
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".FinalShop.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// 6. Seed roles + admin user at startup
using (var scope = app.Services.CreateScope())
{
    await DbInitializer.SeedAsync(scope.ServiceProvider);
}

// 7. Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();            
app.MapRazorPages();             
app.MapControllerRoute(          
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
