using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using TaskManager.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var IsDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

if (!IsDevelopment)
{
    var connectionString = GetHerokuConnectionString();
    builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
}

static string GetHerokuConnectionString()
{
    string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    var databaseUri = new Uri(connectionUrl);

    string db = databaseUri.LocalPath.TrimStart('/');
    string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);

    return $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
}

builder.Services.AddDbContext<AppDbContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext")
        ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 8;
    options.SignIn.RequireConfirmedEmail = true;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(2));

builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/Auth/Login");

builder.Services.AddMailKit(options => options.UseMailKit(builder.Configuration.GetSection("Email").Get<MailKitOptions>()));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddMvc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();

    app.UseMigrationsEndPoint();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AppDbContext>();
    var userMgr = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();

    //context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    var adminRole = new IdentityRole("Admin");
    if (!context.Roles.Any())
    {
        roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();
    }

    if (!context.Users.Any(x => x.UserName == "admin"))
    {
        var adminUser = new IdentityUser()
        {
            UserName = "admin",
            Email = "admin@test.com"
        };
        userMgr.CreateAsync(adminUser, "admin123").GetAwaiter().GetResult();

        userMgr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
    }
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
