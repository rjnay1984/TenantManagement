using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Unit
    {
        public int UnitId { get; set; }
        [Required]
        public string UnitNumber { get; set; }
        [Required]
        public int Bedrooms { get; set; }
        [Required]
        public int Bathrooms { get; set; }
        public string Description { get; set; }
        public int BuildingId { get; set; }
        public Building Building { get; set; }
        public string[] Tenants { get; set; }
    }
}
