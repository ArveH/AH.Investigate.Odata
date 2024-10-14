using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Internal;
using System.Linq.Expressions;
#pragma warning disable EF1001

namespace Pg.Api;

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