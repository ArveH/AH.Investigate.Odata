using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
#pragma warning disable EF1001

namespace Pg.Api;

public class CustomQuerySqlGeneratorFactory(
    QuerySqlGeneratorDependencies dependencies,
    IRelationalTypeMappingSource typeMappingSource,
    INpgsqlSingletonOptions npgsqlSingletonOptions)
    : IQuerySqlGeneratorFactory
{
    public virtual QuerySqlGenerator Create()
        => new CustomQuerySqlGenerator(
            dependencies,
            typeMappingSource,
            npgsqlSingletonOptions.ReverseNullOrderingEnabled,
            npgsqlSingletonOptions.PostgresVersion);
}