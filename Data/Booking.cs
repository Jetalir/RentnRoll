using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RentnRoll.Data
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }
        [Required]
        public string UserID { get; set; } // Foreign key from IdentityUser
        [ForeignKey("UserID")]
        public IdentityUser User { get; set; }
        [Required]
        public int VehicleID { get; set; } // Foreign key from Car
        [ForeignKey("VehicleID")]
        public Vehicle Vehicle { get; set; }
        [Required]
        public DateTime PickupDate { get; set; }
        [Required]
        public DateTime ReturnDate { get; set; }
        public decimal TotalCost { get; set; }
        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Canceled
        public DateTime BookingDate { get; set; } = DateTime.Now;

         
    }
}
