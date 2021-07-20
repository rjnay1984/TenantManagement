using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Data
{
    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions<APIContext> options) : base(options)
        {
        }

        public DbSet<Building> Buildings { get; set; }
        public DbSet<Unit> Units { get; set; }
    }
}
