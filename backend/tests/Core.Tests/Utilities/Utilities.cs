using Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Tests
{
    public class Utilities
    {
        public static DbContextOptions<IdentityContext> TestIdentityContextOptions()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

             var builder = new DbContextOptionsBuilder<IdentityContext>()
                .UseInMemoryDatabase("Identity")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}
