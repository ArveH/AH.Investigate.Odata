namespace Pg.Api.Database;

public class ClientContext: DbContext
{
    public const string CollationName = "en_ci_as";

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

        modelBuilder.Entity<Client>().HasData([
            new Client {Id = 1, Name = "Client 1", Description = "The first client"},
            new Client {Id = 2, Name = "Client 2", Description = "The second client"},
            new Client {Id = 3, Name = "Another", Description = "This is another client"}
        ]);
    }
}