// EF1001: NpgsqlQuerySqlGenerator is an internal API that supports the Entity Framework
// Core infrastructure and not subject to the same compatibility standards as public APIs.
// It may be changed or removed without notice in any release.
#pragma warning disable EF1001 

namespace Pg.Api.CustomOdata;

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