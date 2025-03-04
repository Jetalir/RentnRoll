using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using RentnRoll.Data;

namespace RentnRoll
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("User"));
        }
        public static async Task SeedVehiclesAsync(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                // Om det redan finns bilar, gör inget
                if (context.Vehicles.Any())
                {
                    return;
                }

                // Skapa 10 bilar
                var cars = new Vehicle[]
                {
                new Vehicle { Brand = "Volvo", Model = "XC60", Type = "SUV", Year = 2022, PricePerDay = 750, TransmissionType = "Automat", Description = "Lyxig SUV", ImageURL = "https://example.com/volvo.jpg" },
                new Vehicle { Brand = "BMW", Model = "3-serie", Type = "Sedan", Year = 2021, PricePerDay = 800, TransmissionType = "Automat", Description = "Sportig och elegant", ImageURL = "https://example.com/bmw.jpg" },
                new Vehicle { Brand = "Tesla", Model = "Model 3", Type = "Elbil", Year = 2023, PricePerDay = 900, TransmissionType = "Automat", Description = "Helt elektrisk bil", ImageURL = "https://example.com/tesla.jpg" },
                new Vehicle { Brand = "Audi", Model = "A4", Type = "Sedan", Year = 2020, PricePerDay = 700, TransmissionType = "Automat", Description = "Bekväm och tystgående", ImageURL = "https://example.com/audi.jpg" },
                new Vehicle { Brand = "Mercedes", Model = "C-Klass", Type = "Sedan", Year = 2019, PricePerDay = 850, TransmissionType = "Automat", Description = "Lyxig och stilren", ImageURL = "https://example.com/mercedes.jpg" },
                new Vehicle { Brand = "Volkswagen", Model = "Golf", Type = "Halvkombi", Year = 2021, PricePerDay = 600, TransmissionType = "Manuell", Description = "Praktisk och bränslesnål", ImageURL = "https://example.com/vw.jpg" },
                new Vehicle { Brand = "Ford", Model = "Mustang", Type = "Sportbil", Year = 2022, PricePerDay = 1100, TransmissionType = "Automat", Description = "Kraftfull och snabb", ImageURL = "https://example.com/mustang.jpg" },
                new Vehicle { Brand = "Honda", Model = "Civic", Type = "Sedan", Year = 2020, PricePerDay = 650, TransmissionType = "Manuell", Description = "Pålitlig och prisvärd", ImageURL = "https://example.com/honda.jpg" },
                new Vehicle { Brand = "Hyundai", Model = "Tucson", Type = "SUV", Year = 2021, PricePerDay = 720, TransmissionType = "Automat", Description = "Rymlig och modern", ImageURL = "https://example.com/tucson.jpg" },
                new Vehicle { Brand = "Nissan", Model = "Leaf", Type = "Elbil", Year = 2022, PricePerDay = 780, TransmissionType = "Automat", Description = "Miljövänlig elbil", ImageURL = "https://example.com/leaf.jpg" }
                };

                context.Vehicles.AddRange(cars);
                await context.SaveChangesAsync();
            }
        }
        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                // Hämta UserManager
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

                // Om användaren redan finns, gör inget
                if (context.Users.Any(u => u.Email == "user@epost.se"))
                {
                    return;
                }
                else
                {
                    // Skapa användare
                    var user = new IdentityUser
                    {
                        UserName = "user@epost.se",
                        Email = "user@epost.se",
                        EmailConfirmed = true
                    };
                    // Skapa användaren med ett lösenord
                    var result = await userManager.CreateAsync(user, "Password123!");
                }
                if (context.Users.Any(u => u.Email == "admin@epost.se"))
                {
                    return;
                }
                else
                {
                    // Skapa användare
                    var useradmin = new IdentityUser
                    {
                        UserName = "admin@epost.se",
                        Email = "admin@epost.se",
                        EmailConfirmed = true
                    };
                    // Skapa användaren med ett lösenord
                    var result = await userManager.CreateAsync(useradmin, "Password123!");
                    if (result.Succeeded)
                    {
                        // Lägg till rollen "Admin" till användaren
                        await userManager.AddToRoleAsync(useradmin, "Admin");
                    }
                }
                await context.SaveChangesAsync();
            }
        }

    }
}

