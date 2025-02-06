using ManagementUser.WebApp.Data;
using ManagementUser.WebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/*var factory = new IdentityAppDbContextFactory();
using (var context = factory.CreateDbContext(args))
{
    context.Database.Migrate();
}*/

builder.Services.AddDbContext<IdentityAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>(opt =>
    {
        opt.Lockout.MaxFailedAccessAttempts = 5;
        opt.SignIn.RequireConfirmedPhoneNumber = false;
        opt.User.RequireUniqueEmail = true;
        opt.Password.RequireDigit = true;
        opt.Password.RequiredLength = 6;
        opt.Password.RequireNonAlphanumeric = true;
        opt.Password.RequireUppercase = true;
        opt.Password.RequireLowercase = true;
    })
    .AddEntityFrameworkStores<IdentityAppDbContext>()
    .AddUserManager<UserManager<IdentityUser<Guid>>>()
    .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
    .AddSignInManager<SignInManager<IdentityUser<Guid>>>()
    .AddDefaultTokenProviders();

builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
    opt =>
    {
        opt.LoginPath = new PathString("/auth/login");
        //opt.AccessDeniedPath = new PathString("/acesso-negado");
    });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o => {
        o.LoginPath = new PathString("/auth/login");
        //o.AccessDeniedPath = new PathString("/acesso-negado");
    });

builder.Services.AddSession();

builder.Services.AddControllersWithViews()
    .AddSessionStateTempDataProvider();
builder.Services.AddRazorPages()
    .AddSessionStateTempDataProvider();

builder.Services.AddHostedService<RoleWorker>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();

    //app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseSession();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapGet("/", context =>
        {
            context.Response.Redirect("/auth/register");
            return Task.CompletedTask;
        });

        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Auth}/{action=Register}/{id?}");

        endpoints.MapControllerRoute(
            name: "auth",
            pattern: "auth/{action=Login}",
            defaults: new { controller = "Auth" });

        endpoints.MapControllerRoute(
            name: "manager",
            pattern: "manager/{action=index}",
            defaults: new { controller = "Manager" });

        endpoints.MapRazorPages();
    });
    
    app.MapDefaultControllerRoute();

    app.MapRazorPages();
    app.Run();
}