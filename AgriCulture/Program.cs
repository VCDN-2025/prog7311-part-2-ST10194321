using AgriCulture.Data;
using AgriCulture.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. MySQL-backed Identity DbContext
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
    opts.UseMySql(conn, ServerVersion.AutoDetect(conn))
);

// 2. Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
{
    opts.SignIn.RequireConfirmedAccount = false;
    opts.Password.RequireDigit = true;
    opts.Password.RequireLowercase = true;
    opts.Password.RequireUppercase = true;
    opts.Password.RequireNonAlphanumeric = true;
    opts.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 3. MVC + Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// 4. Create roles and default employee
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Create roles if they don't exist
    if (!await roleManager.RoleExistsAsync("Employee"))
    {
        await roleManager.CreateAsync(new IdentityRole("Employee"));
    }
    if (!await roleManager.RoleExistsAsync("Farmer"))
    {
        await roleManager.CreateAsync(new IdentityRole("Farmer"));
    }

    // Create default employee if it doesn't exist
    var defaultEmployeeEmail = "employee@agriculture.com";
    var defaultEmployee = await userManager.FindByEmailAsync(defaultEmployeeEmail);
    if (defaultEmployee == null)
    {
        var employee = new ApplicationUser
        {
            UserName = defaultEmployeeEmail,
            Email = defaultEmployeeEmail,
            FirstName = "Default",
            LastName = "Employee",
            UserType = "Employee",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(employee, "Employee123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(employee, "Employee");
        }
    }
}

// 5. Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 6. Endpoint mapping
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Ensure Employee role exists
    if (!await roleManager.RoleExistsAsync("Employee"))
        await roleManager.CreateAsync(new IdentityRole("Employee"));

    // Add a new employee if not exists
    var email = "newemployee@demo.com";
    var user = await userManager.FindByEmailAsync(email);
    if (user == null)
    {
        user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = "New",
            LastName = "Employee",
            UserType = "Employee",
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(user, "Employee1229!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Employee");
        }
    }
}

app.Run();
