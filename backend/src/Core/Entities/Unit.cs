namespace Core.Entities
{
    public class Unit
    {
        public int UnitId { get; set; }
        public string UnitNumber { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string Description { get; set; }
        public int BuildingId { get; set; }
        public Building Building { get; set; }
    }
}
