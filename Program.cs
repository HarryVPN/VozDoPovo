using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using VozDoPovo.Data;

var builder = WebApplication.CreateBuilder(args);

var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BancoContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase"));
});

//builder.Services.AddScoped<IContatoRepositorio, ContatoRepositorio>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
    });

//builder.Services.AddDbContext<BancoContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("BolConnection"));
//});
//builder.Services.AddIdentity<FrameworkSqlServer, IdentityRole>()
//    .AddEntityFrameworkStores<BancoContext>()
//    .AddDefaultTokenProviders();

var app = builder.Build();

var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
};

app.UseCookiePolicy(cookiePolicyOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.MapRazorPages();
app.MapDefaultControllerRoute();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();