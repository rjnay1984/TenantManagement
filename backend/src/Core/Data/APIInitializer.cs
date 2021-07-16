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
        }
    }
}
