using Microsoft.EntityFrameworkCore;

namespace OpenHealthTrackerApi.Data;

public class OHTDbContext : DbContext
{
    private IConfiguration _config;

    public OHTDbContext(IConfiguration config)
    {
        _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql($"Host={_config.GetValue<string>("Database:Host")};Database={_config.GetValue<string>("Database:Database")};Username={_config.GetValue<string>("Database:Username")};Password={_config.GetValue<string>("Database:Password")}");
}