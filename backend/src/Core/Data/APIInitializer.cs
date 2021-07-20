using Core.Entities;

namespace Core.Data
{
    public static class APIInitializer
    {
        public static void Initialize(APIContext context)
        {
            var buildings = new Building[]
            {
                new Building() { Name = "Building One", Address = "123 First St", City = "Des Moines", State = "Iowa", ZipCode = "50309" },
                new Building() { Name = "Building Two", Address = "456 Second St", City = "West Des Moines", State = "Iowa", ZipCode = "50266" }
            };

            context.AddRange(buildings);
            context.SaveChanges();

            var rooms = new Unit[]
            {
                new Unit() { UnitNumber = "1A", BuildingId = 1 },
                new Unit() { UnitNumber = "2B", BuildingId = 1 },
                new Unit() { UnitNumber = "3C", BuildingId = 1 },
                new Unit() { UnitNumber = "4D", BuildingId = 2 },
                new Unit() { UnitNumber = "5E", BuildingId = 2 },
                new Unit() { UnitNumber = "6F", BuildingId = 2 }
            };

            context.AddRange(rooms);
            context.SaveChanges();
        }
    }
}
