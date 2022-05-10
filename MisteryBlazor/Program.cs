using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MisteryBlazor.Areas.Identity;
using MisteryBlazor.Data;
using MisteryBlazor.Data.Context;
using MisteryBlazor.Data.Seeder;
using MisteryBlazor.Data.User;
using MisteryBlazor.Services;
using MisteryBlazor.Services.DAL;
using MisteryBlazor.Services.DataManager;
using MudBlazor;
using MudBlazor.Services;
using Animation = BlazorContextMenu.Animation;

var builder = WebApplication.CreateBuilder(args);
// var connectionString = builder.Configuration.GetConnectionString("ReleaseConnection");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Host.ConfigureLogging(logging =>
{
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<MisteryIdentityUser>((IdentityOptions options) => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<MisteryIdentityUser>>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services
    .AddScoped<GroupsManager>().AddScoped<ChannelsManager>()
    .AddScoped<UserDataService>().AddScoped<AuthorizationManager>()
    .AddScoped<GroupDataService>().AddAntDesign();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

builder.Services.Configure<IdentityOptions>(options =>
{

});

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
    config.SnackbarConfiguration.VisibleStateDuration = 3000;
    config.SnackbarConfiguration.HideTransitionDuration = 200;
    config.SnackbarConfiguration.ShowTransitionDuration = 200;
});

builder.Services.AddBlazorContextMenu(options =>
{
    options.ConfigureTemplate("ContextMenuTemplate", template =>
    {
        template.MenuHiddenCssClass = "ContextMenuTemplate-Hidden";
        template.Animation = Animation.FadeIn;
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
    var logger = services.GetService<ILoggerFactory>().CreateLogger<Program>();
    Seeder seed = new Seeder(services, logger);
    await seed.Seed();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.UseStatusCodePages(
    async options =>
    {
        if (options.HttpContext.Response.StatusCode == 404)
        {
            options.HttpContext.Response.Redirect("/Pages/_LongShared");
        }
    }
);

app.Run();
