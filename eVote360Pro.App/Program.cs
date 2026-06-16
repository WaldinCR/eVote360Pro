using eVote360Pro.App.Middlewares;
using eVote360Pro.Core.Application;
using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Infrastructure.Persistence;
using eVote360Pro.Infrastructure.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromMinutes(60);
    opt.Cookie.HttpOnly = true;
});

builder.Services.AddPersistenceLayerIoc(builder.Configuration);
builder.Services.AddApplicationLayerIoc();
builder.Services.AddSharedLayerIoc(builder.Configuration);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUserSession, UserSession>();

builder.Services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
{
    options.AreaViewLocationFormats.Clear();
    options.AreaViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
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
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "login_route",
    pattern: "Login/{action=Index}/{id?}",
    defaults: new { area = "", controller = "Login" })
    .WithStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "elector_route",
    pattern: "Elector/{action=Index}/{id?}",
    defaults: new { area = "Elector", controller = "Elector" })
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}") 
    .WithStaticAssets();


await app.RunAsync();
