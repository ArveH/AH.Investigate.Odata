using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using Microsoft.OData.UriParser;

namespace Pg.Api.CustomOdata;

public class CustomFilterBinder : FilterBinder
{
    protected override Expression BindStartsWith(
        SingleValueFunctionCallNode node,
        QueryBinderContext context)
    {
        CheckArgumentNull(node, context, "startswith");

        var nodeParameters = node.Parameters.ToList();

        if (nodeParameters.Count != 2)
        {
            throw new InvalidOperationException("BindStartsWith node doesn't have 2 parameters");
        }

        var matchExpression = Bind(nodeParameters[0], context);
        Contract.Assert(matchExpression.Type == typeof(string));

        var patternString = (nodeParameters[1] as ConstantNode)?.Value;
        if (patternString == null)
        {
            throw new InvalidOperationException("BindStartsWith patternString is not a ConstantNode");
        }
        var patternExpression = Expression.Constant(patternString + "%");

        var collateMethodInfo = typeof(RelationalDbFunctionsExtensions)
                                    .GetMethod(nameof(RelationalDbFunctionsExtensions.Collate))
                                ?? throw new InvalidOperationException("MethodInfo for 'RelationalDbFunctionsExtensions.Collate' not found");
        var iLikeMethodInfo = typeof(NpgsqlDbFunctionsExtensions)
                                  .GetMethod(
                                      nameof(NpgsqlDbFunctionsExtensions.ILike),
                                      [typeof(DbFunctions), typeof(string), typeof(string)])
                              ?? throw new InvalidOperationException("MethodInfo for 'NpgsqlDbFunctionsExtensions.ILike' not found");

        var collateExpression = Expression.Call(
            collateMethodInfo.MakeGenericMethod(typeof(string)),
            Expression.Constant(EF.Functions),
            matchExpression,
            Expression.Constant(ClientContext.CollationNameForLike)
        );

        var likeExpression = Expression.Call(
            iLikeMethodInfo,
            Expression.Constant(EF.Functions),
            collateExpression,
            patternExpression
        );

        return likeExpression;
    }
    
    private static void CheckArgumentNull(SingleValueFunctionCallNode node, QueryBinderContext context, string nodeName)
    {
        if (node == null || node.Name != nodeName)
        {
            throw new ArgumentNullException(nameof(node));
        }

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
    }
}