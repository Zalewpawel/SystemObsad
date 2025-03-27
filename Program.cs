using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sedziowanie.Data;
using Sedziowanie.Models;
using Sedziowanie.Services;
using Sedziowanie.Services.Interfaces;
using System;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IMeczService, MeczService>();
builder.Services.AddScoped<INiedyspozycjaService, NiedyspozycjaService>();
builder.Services.AddScoped<IRozgrywkiService, RozgrywkiService>();
builder.Services.AddScoped<ISedziaService, SedziaService>();
builder.Services.AddDbContext<DBObsadyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ObsadyDB")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DBObsadyContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.InitializeAsync(services);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
app.MapRazorPages();
app.Run();

