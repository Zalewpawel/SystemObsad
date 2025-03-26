using Microsoft.EntityFrameworkCore;
using Sedziowanie.Data;
using Sedziowanie.Services;
using Sedziowanie.Services.Interfaces;
using System;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IMeczService, MeczService>();
builder.Services.AddScoped<INiedyspozycjaService, NiedyspozycjaService>();
builder.Services.AddScoped<IRozgrywkiService, RozgrywkiService>();
builder.Services.AddScoped<ISedziaService, SedziaService>();
builder.Services.AddDbContext<DBObsadyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ObsadyDB")));


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); 

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Sedzia}/{action=Show}/{id?}");

app.Run();
