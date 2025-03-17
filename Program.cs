using Microsoft.EntityFrameworkCore;
using Sedziowanie.Data;
using System;

var builder = WebApplication.CreateBuilder(args);

// Dodajemy us³ugi do kontenera
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DBObsadyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ObsadyDB")));

// Dodajemy us³ugê sesji
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Konfiguracja potoku ¿¹dañ HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // W³¹czamy obs³ugê sesji

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Sedzia}/{action=Show}/{id?}");

app.Run();
