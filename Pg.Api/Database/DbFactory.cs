namespace Pg.Api.Database;

public class DbFactory : IDbFactory
{
    private readonly DbContextOptions<ClientContext> _options;

    public DbFactory(IConfiguration config, ILoggerFactory loggerFactory)
    {
        ConnectionString = config.GetConnectionString("Default");

        _options = new DbContextOptionsBuilder<ClientContext>()
            .UseNpgsql(ConnectionString, pgOptions =>
            {
                pgOptions.EnableRetryOnFailure();
            })
            .EnableSensitiveDataLogging()
            .UseLoggerFactory(loggerFactory)
            //.UseLowerCaseNamingConvention()
            .Options;

    }

    public string? ConnectionString { get; set; }

    public ClientContext CreateContext()
    {
        return new ClientContext(_options);
    }
}