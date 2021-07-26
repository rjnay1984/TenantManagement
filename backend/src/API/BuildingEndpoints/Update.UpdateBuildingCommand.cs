using System.ComponentModel.DataAnnotations;

namespace API.BuildingEndpoints
{
    public class UpdateBuildingCommand
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Landlord { get; set; }
    }
}
