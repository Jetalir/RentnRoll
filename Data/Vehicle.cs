using System.ComponentModel.DataAnnotations;

namespace RentnRoll.Data
{
    public class Vehicle
    {
        [Key]
        public int VehicleID { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        [Required]
        public decimal PricePerDay { get; set; }
        public string TransmissionType { get; set; }
        public bool Availibility { get; set; } = true;
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
    }
}
