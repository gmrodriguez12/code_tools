using Microsoft.EntityFrameworkCore;

namespace Common.SQLServerRepository.Context
{
    public class GenericDbContext : DbContext
    {
        public GenericDbContext(DbContextOptions<GenericDbContext> options) : base(options)
        {
        }

        public DbSet<T> GetDbSet<T>() where T : class
        {
            return base.Set<T>();
        }
    }
}