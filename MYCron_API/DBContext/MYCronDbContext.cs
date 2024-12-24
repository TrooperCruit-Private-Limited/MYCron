using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MYCentralModels;
using System.Text.Json;

namespace MYCron_API.DBContext
{
    public class MYCronDbContext(DbContextOptions<MYCronDbContext> options) : DbContext(options)
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<EmployeeOnSite> EmployeesOnSite { get; set; }
        public DbSet<VisitUsInformationModel> VisitUsInformation { get; set; }
        public DbSet<SiteVisits> SiteVisits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var boolKeyValueModelConverter = new ValueConverter<List<BoolKeyValueModel>, string>(
                v => JsonSerializer.Serialize(v, CommonAssets.JsonSerializerOptions), // Convert List<BoolKeyValueModel> to JSON string for storage
                v => JsonSerializer.Deserialize<List<BoolKeyValueModel>>(v, CommonAssets.JsonSerializerOptions) // Convert JSON string to List<BoolKeyValueModel>
            );
        }
    }
}
