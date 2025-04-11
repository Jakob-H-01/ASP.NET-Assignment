using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("AlphaDB")));
builder.Services.AddIdentity<MemberEntity, IdentityRole>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = "/login";
    x.AccessDeniedPath = "/admin/login";
    x.Cookie.HttpOnly = true;
    x.Cookie.IsEssential = true;
    x.ExpireTimeSpan = TimeSpan.FromHours(1);
    x.SlidingExpiration = true;
    x.Cookie.SameSite = SameSiteMode.None;
    x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.AddAuthentication(x =>
{
    x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie().AddGoogle(x =>
{
    x.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    x.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
});

builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMemberService, MemberService>();

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Standard", "Admin" };

    foreach (var roleName in roleNames)
    {
        var exists = await roleManager.RoleExistsAsync(roleName);
        if (!exists)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<MemberEntity>>();
    var member = new MemberEntity { UserName = "admin@domain.com", Email = "admin@domain.com", FirstName = "Admin", LastName = "Member" };

    var exists = await userManager.FindByEmailAsync(member.Email);
    if (exists == null)
    {
        var result = await userManager.CreateAsync(member, "Admin123!");
        if (result.Succeeded)
            await userManager.AddToRoleAsync(member, "Admin");
    }
}

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();