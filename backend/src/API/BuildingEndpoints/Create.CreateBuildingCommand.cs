using System.ComponentModel.DataAnnotations;

namespace API.BuildingEndpoints
{
    public class CreateBuildingCommand
    {
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Landlord { get; set; }
    }
}
