using BpstEdu.Data;
using BpstEdu.DBModels;
using BpstEdu.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddScoped<BpstEdu.Services.IStudentApplicationService, BpstEdu.Services.StudentApplicationService>();

builder.Services.AddScoped<IUserServiceBAL, UserServiceBAL>();

builder.Services.AddScoped<IStudentServiceBAL, StudentServiceBAL>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConStr")));
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<AppDbContext>();


var app = builder.Build();

// SeedAdminUser(app);

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

app.UseAuthentication();
app.UseAuthorization();

 

app.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();

static void SeedAdminUser(IHost app)
{
    var scopeFactory = app.Services.GetService<IServiceScopeFactory>();
    using (var scope = scopeFactory.CreateScope())
    {
        var userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

        if (!roleManager.RoleExistsAsync("Admin").Result)
        {
            roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
        }

        if (userManager.FindByNameAsync("admin").Result == null)
        {
            var admin = new AppUser
            {
                UserName = "admin@bpst.com",
                Email = "admin@bpst.com",
                PhoneNumber = "1234567890"
            };
            var result = userManager.CreateAsync(admin, "Admin@123").Result;
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(admin, "Admin").Wait();
            }
        }
    }
}
