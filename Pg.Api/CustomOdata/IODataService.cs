namespace Pg.Api.CustomOdata;

public interface IODataService
{
    IQueryable<TEntity> ApplyTo<TEntity>(
        IQueryable<TEntity> query,
        HttpRequest httpRequest);
}