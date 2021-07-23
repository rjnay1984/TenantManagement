using Core.Entities;

namespace Infrastructure.Data
{
    public static class APIInitializer
    {
        public static void Initialize(APIContext context)
        {
            var buildings = new Building[]
            {
                new Building() { Name = "Building One", Address = "123 First St", City = "Des Moines", State = "Iowa", ZipCode = "50309", Landlord = "bobsmith@email.com" },
                new Building() { Name = "Building Two", Address = "456 Second St", City = "West Des Moines", State = "Iowa", ZipCode = "50266", Landlord = "bobsmith@email.com" }
            };

            context.AddRange(buildings);
            context.SaveChanges();

            var rooms = new Unit[]
            {
                new Unit() { UnitNumber = "1A", Bedrooms = 2, Bathrooms = 1, BuildingId = 1 },
                new Unit() { UnitNumber = "2B", Bedrooms = 1, Bathrooms = 1, BuildingId = 1 },
                new Unit() { UnitNumber = "3C", Bedrooms = 3, Bathrooms = 2, BuildingId = 1, Tenants = new[] { "aaron@email.com", "david@email.com" } },
                new Unit() { UnitNumber = "4D", Bedrooms = 2, Bathrooms = 1, BuildingId = 2 },
                new Unit() { UnitNumber = "5E", Bedrooms = 1, Bathrooms = 1, BuildingId = 2, Tenants = new[] { "kurt@email.com" } },
                new Unit() { UnitNumber = "6F", Bedrooms = 3, Bathrooms = 1, BuildingId = 2 }
            };

            context.AddRange(rooms);
            context.SaveChanges();
        }
    }
}
