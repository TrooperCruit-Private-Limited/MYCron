using Microsoft.EntityFrameworkCore;
using MYCentralModels.SQLite;

namespace MYCron_APP.DbContextLite
{
    public class SqLiteDbContext(DbContextOptions<SqLiteDbContext> options) : DbContext(options)
    {
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<OutOfSessionPage> OutOfSessionPages { get; set; }
        public DbSet<InternalUrl> InternalUrls { get; set; }
    }
}
