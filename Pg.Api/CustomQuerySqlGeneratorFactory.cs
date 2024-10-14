using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace Pg.Api;

public class CustomQuerySqlGeneratorFactory : IQuerySqlGeneratorFactory
{
    private readonly QuerySqlGeneratorDependencies _dependencies;
    private readonly IRelationalTypeMappingSource _typeMappingSource;
    private readonly INpgsqlSingletonOptions _npgsqlSingletonOptions;

    public CustomQuerySqlGeneratorFactory(
        QuerySqlGeneratorDependencies dependencies,
        IRelationalTypeMappingSource typeMappingSource,
        INpgsqlSingletonOptions npgsqlSingletonOptions)
    {
        _dependencies = dependencies;
        _typeMappingSource = typeMappingSource;
        _npgsqlSingletonOptions = npgsqlSingletonOptions;
    }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public virtual QuerySqlGenerator Create()
        => new CustomQuerySqlGenerator(
            _dependencies,
            _typeMappingSource,
            _npgsqlSingletonOptions.ReverseNullOrderingEnabled,
            _npgsqlSingletonOptions.PostgresVersion);
}