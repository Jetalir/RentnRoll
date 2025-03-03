using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RentnRoll.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        //Lägg till tabeller
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }
        
    }
}
