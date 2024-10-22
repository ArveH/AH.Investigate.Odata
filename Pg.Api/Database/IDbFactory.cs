namespace Pg.Api.Database;

public interface IDbFactory
{
    string? ConnectionString { get; set; }
    ClientContext CreateContext();
}