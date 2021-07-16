using Microsoft.EntityFrameworkCore;

namespace Core.Data
{
    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions<APIContext> options) : base(options)
        {
        }
    }
}
