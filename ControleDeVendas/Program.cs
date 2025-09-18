using Microsoft.EntityFrameworkCore;
using ControleDeVendas.Data;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ControleDeVendasContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ControleDeVendasContext") ?? throw new InvalidOperationException("Connection string 'ControleDeVendasContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ControleDeVendasContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("ControleDeVendasContext"),
        npgsqlOptions => npgsqlOptions.MigrationsAssembly("ControleDeVendas")));

var app = builder.Build();
var ptBR = new CultureInfo("pt-BR");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(ptBR),
    SupportedCultures = new List<CultureInfo> { ptBR },
    SupportedUICultures = new List<CultureInfo> { ptBR }
};

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
