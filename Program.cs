using RentnRoll;
using RentnRoll.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect to a AccessDenied page if the user doesn't have admin privlages
});

// Add services to the container.
builder.Services.AddRazorPages();



var app = builder.Build();


var scope = app.Services.CreateScope();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
RoleSeeder.SeedRolesAsync(roleManager).Wait();
using (var scopeTwo = app.Services.CreateScope())
{
	var loadData = scopeTwo.ServiceProvider;
	await RoleSeeder.SeedVehiclesAsync(loadData);
	await RoleSeeder.SeedUsersAsync(loadData);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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



app.MapRazorPages();

app.Run();
