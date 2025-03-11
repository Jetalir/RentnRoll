using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
					new Vehicle { Brand = "Volvo", Model = "XC60", Type = "SUV", Year = 2022, PricePerDay = 750, TransmissionType = "Automatic", Description = "Luxury SUV with advanced safety features.", ImageURL = "https://d2ivfcfbdvj3sm.cloudfront.net/7fc965ab77efe6e0fa62e4ca1ea7673bb65a4856091e3d8e88cb10/stills_0640_png/MY2022/50972/50972_st0640_116.png" },
					new Vehicle { Brand = "BMW", Model = "3 Series", Type = "Sedan", Year = 2021, PricePerDay = 800, TransmissionType = "Automatic", Description = "Sporty and elegant sedan.", ImageURL = "https://dbhdyzvm8lm25.cloudfront.net/stills_0640_png/MY2021/14447/14447_st0640_116.png" },
					new Vehicle { Brand = "Toyota", Model = "Camry", Type = "Sedan", Year = 2023, PricePerDay = 700, TransmissionType = "Automatic", Description = "Reliable and comfortable midsize sedan.", ImageURL = "https://d2ivfcfbdvj3sm.cloudfront.net/7fc965ab77efe6e0fa62e4ca1ea7673bb65b4454021e3d8e88cb10/stills_0640_png/MY2023/51559/51559_st0640_116.png" },
					new Vehicle { Brand = "Audi", Model = "A4", Type = "Sedan", Year = 2020, PricePerDay = 700, TransmissionType = "Automatic", Description = "Comfortable and quiet ride.", ImageURL = "https://dbhdyzvm8lm25.cloudfront.net/stills_0640_png/MY2020/14275/14275_st0640_116.png" },
					new Vehicle { Brand = "Mercedes-Benz", Model = "C-Class", Type = "Sedan", Year = 2019, PricePerDay = 850, TransmissionType = "Automatic", Description = "Luxury sedan with stylish design.", ImageURL = "https://d2ivfcfbdvj3sm.cloudfront.net/7fc965ab77efe6e0fa62e4ca1ea7673bb65a46530e1e3d8e88cb10/stills_0640_png/MY2022/50725/50725_st0640_116.png" },
					new Vehicle { Brand = "Volkswagen", Model = "Golf", Type = "Hatchback", Year = 2021, PricePerDay = 600, TransmissionType = "Manual", Description = "Practical and fuel-efficient hatchback.", ImageURL = "https://dbhdyzvm8lm25.cloudfront.net/stills_0640_png/MY2021/14775/14775_st0640_116.png" },
					new Vehicle { Brand = "Ford", Model = "Mustang", Type = "Sports Car", Year = 2022, PricePerDay = 1100, TransmissionType = "Automatic", Description = "Powerful and fast muscle car.", ImageURL = "https://d2ivfcfbdvj3sm.cloudfront.net/7fc965ab77efe6e0fa62e4ca1ea7673bb65b49530f1e3d8e88cb10/stills_0640_png/MY2022/51824/51824_st0640_116.png" },
					new Vehicle { Brand = "Honda", Model = "Civic", Type = "Sedan", Year = 2020, PricePerDay = 650, TransmissionType = "Manual", Description = "Reliable and budget-friendly compact car.", ImageURL = "https://dbhdyzvm8lm25.cloudfront.net/stills_0640_png/MY2020/14054/14054_st0640_116.png" },
					new Vehicle { Brand = "Hyundai", Model = "Tucson", Type = "SUV", Year = 2021, PricePerDay = 720, TransmissionType = "Automatic", Description = "Spacious and modern compact SUV.", ImageURL = "https://dbhdyzvm8lm25.cloudfront.net/stills_0640_png/MY2021/14485/14485_st0640_116.png" },
					new Vehicle { Brand = "Chevrolet", Model = "Malibu", Type = "Sedan", Year = 2022, PricePerDay = 780, TransmissionType = "Automatic", Description = "Comfortable and stylish midsize sedan.", ImageURL = "https://d2ivfcfbdvj3sm.cloudfront.net/7fc965ab77efe6e0fa62e4ca1ea7673bb65a41520d1e3d8e88cb10/stills_0640_png/MY2022/50036/50036_st0640_116.png" }
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
