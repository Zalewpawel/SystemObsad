using Microsoft.EntityFrameworkCore;
using Sedziowanie.Data;
using System;

var builder = WebApplication.CreateBuilder(args);

// Dodajemy us�ugi do kontenera
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DBObsadyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ObsadyDB")));

// Dodajemy us�ug� sesji
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Konfiguracja potoku ��da� HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // W��czamy obs�ug� sesji

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Sedzia}/{action=Show}/{id?}");

app.Run();
