namespace OpenHealthTrackerApi.Data;

public class DbFactory
{
    private IConfiguration _config;

    public DbFactory(IConfiguration config)
    {
        _config = config;
    }

    public OHTDbContext OHT()
    {
        return new OHTDbContext(_config);
    }
}