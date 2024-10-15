// EF1001: NpgsqlQuerySqlGenerator is an internal API that supports the Entity Framework
// Core infrastructure and not subject to the same compatibility standards as public APIs.
// It may be changed or removed without notice in any release.
#pragma warning disable EF1001 

namespace Pg.Api.CustomOdata;

public class CustomQuerySqlGenerator : NpgsqlQuerySqlGenerator
{
    /// <inheritdoc />
    public CustomQuerySqlGenerator(
        QuerySqlGeneratorDependencies dependencies,
        IRelationalTypeMappingSource typeMappingSource,
        bool reverseNullOrderingEnabled,
        Version postgresVersion)
        : base(dependencies, typeMappingSource, reverseNullOrderingEnabled, postgresVersion)
    {

    }

    protected override Expression VisitExtension(Expression extensionExpression)
        => extensionExpression switch
        {
            LikeExpression e => VisitLike(e),

            _ => base.VisitExtension(extensionExpression)
        };

    private new Expression VisitLike(LikeExpression likeExpression)
    {
        Visit(likeExpression.Match);
        Sql.Append(" ILIKE ");
        Visit(likeExpression.Pattern);

        if (likeExpression.EscapeChar is not null)
        {
            Sql.Append(" ESCAPE ");
            Visit(likeExpression.EscapeChar);
        }

        Sql.Append($" COLLATE {ClientContext.CollationNameForLike} ");

        return likeExpression;
    }
}