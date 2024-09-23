namespace Pg.Api.Database;

public class ClientContext: DbContext
{
    private const string CollationName = "en_ci_as";

    public ClientContext(DbContextOptions<ClientContext> options)
        : base(options)
    {
        
    }

    public DbSet<Client> Clients { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().UseCollation(CollationName);

        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasCollation(CollationName,
                provider: "icu",
                locale: "en-u-ks-level1",
                deterministic: false);
        modelBuilder.HasCollation(CollationName,
                provider: "icu",
                locale: "en-u-ks-level2",
                deterministic: false);
    }
}