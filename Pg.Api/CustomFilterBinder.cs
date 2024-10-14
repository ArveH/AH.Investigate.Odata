using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.OData.UriParser;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

namespace Pg.Api;

public class CustomFilterBinder : FilterBinder
{
#if true
    protected override Expression BindStartsWith(
        SingleValueFunctionCallNode node,
        QueryBinderContext context)
    {
        CheckArgumentNull(node, context, "startswith");

        Expression[] arguments = BindArguments(node.Parameters, context);
        ValidateAllStringArguments(node.Name, arguments);

        Contract.Assert(arguments.Length == 2 && arguments[0].Type == typeof(string) &&
                        arguments[1].Type == typeof(string));

        var collateMethodInfo = typeof(RelationalDbFunctionsExtensions)
                .GetMethod(nameof(RelationalDbFunctionsExtensions.Collate))
            ?? throw new InvalidOperationException("Method 'Collate' not found");
        var collateExpression = Expression.Call(
            collateMethodInfo.MakeGenericMethod(typeof(string)),
            Expression.Constant(EF.Functions),
            arguments[0],
            Expression.Constant(ClientContext.CollationNameForLike)
        );

        var likeExpression = Expression.Call(
            typeof(DbFunctionsExtensions).GetMethod(
                nameof(DbFunctionsExtensions.Like),
                [typeof(DbFunctions), typeof(string), typeof(string)])!,
            Expression.Constant(EF.Functions),
            collateExpression,
            arguments[1]
        );

        return likeExpression;
        //return Expression.Lambda<Func<string, bool>>(likeExpression, (ParameterExpression)arguments[0]);
    }
#else
    protected override Expression BindStartsWith(SingleValueFunctionCallNode node, QueryBinderContext context)
    {
        CheckArgumentNull(node, context, "startswith");

        Expression[] arguments = BindArguments(node.Parameters, context);
        ValidateAllStringArguments(node.Name, arguments);

        Contract.Assert(arguments.Length == 2 && arguments[0].Type == typeof(string) && arguments[1].Type == typeof(string));

        return ExpressionBinderHelper.MakeFunctionCall(ClrCanonicalFunctions.StartsWith, context.QuerySettings, arguments);
    }
#endif

    private static IEnumerable<Expression> ExtractValueFromNullableArguments(IEnumerable<Expression> arguments)
    {
        return arguments.Select(arg => ExtractValueFromNullableExpression(arg));
    }

    public static Expression ExtractValueFromNullableExpression(Expression source)
    {
        return Nullable.GetUnderlyingType(source.Type) != null ? Expression.Property(source, "Value") : source;
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

    private static void ValidateAllStringArguments(string functionName, Expression[] arguments)
    {
        if (arguments.Any(arg => arg.Type != typeof(string)))
        {
            throw new InvalidOperationException(
                string.Format("The '{0}' function cannot be applied to an enumeration-typed argument.", functionName));
        }
    }

    private static Expression RemoveInnerNullPropagation(Expression expression, ODataQuerySettings querySettings)
    {
        Contract.Assert(expression != null);

        if (querySettings.HandleNullPropagation == HandleNullPropagationOption.True)
        {
            // only null propagation generates conditional expressions
            if (expression.NodeType == ExpressionType.Conditional)
            {
                // make sure to skip the DateTime IFF clause
                ConditionalExpression conditionalExpr = (ConditionalExpression)expression;
                if (conditionalExpr.Test.NodeType != ExpressionType.OrElse)
                {
                    expression = conditionalExpr.IfFalse;
                    Contract.Assert(expression != null);

                    if (expression.NodeType == ExpressionType.Convert)
                    {
                        UnaryExpression unaryExpression = expression as UnaryExpression;
                        Contract.Assert(unaryExpression != null);

                        if (Nullable.GetUnderlyingType(unaryExpression.Type) == unaryExpression.Operand.Type)
                        {
                            // this is a cast from T to Nullable<T> which is redundant.
                            expression = unaryExpression.Operand;
                        }
                    }
                }
            }
        }

        return expression;
    }

    protected Expression BindStartsWith_2(
        SingleValueFunctionCallNode node,
        QueryBinderContext context)
    {
        // Check that the function is startsWith
        if (node.Name != "startswith" || node.Parameters.Count() != 2)
        {
            throw new NotSupportedException($"Unsupported function '{node.Name}'");
        }

        // Extract arguments from the OData query
        var propertyNode = node.Parameters.ElementAt(0) as SingleValuePropertyAccessNode;
        var constantNode = node.Parameters.ElementAt(1) as ConstantNode;

        if (propertyNode == null || constantNode == null)
        {
            throw new InvalidOperationException("Invalid startsWith expression");
        }

        // Get the property access expression (e.g., Name)
        var propertyExpression = Bind(propertyNode, context);

        // Get the search text and append the wildcard %
        var searchText = constantNode.Value as string;
        if (searchText == null)
        {
            throw new InvalidOperationException("Invalid search text in startsWith");
        }

        // Create the expression for ILIKE using Postgres syntax
        var searchTextExpression = Expression.Constant($"{searchText}%");

        // Combine the property and the search pattern with ILIKE
        var iLikeMethod = typeof(NpgsqlDbFunctionsExtensions).GetMethod(
            nameof(NpgsqlDbFunctionsExtensions.ILike),
            new[] { typeof(DbFunctions), typeof(string), typeof(string) }
        );

        var iLikeExpression = Expression.Call(
            typeof(NpgsqlDbFunctionsExtensions),
            nameof(NpgsqlDbFunctionsExtensions.ILike),
            Type.EmptyTypes,
            propertyExpression,
            searchTextExpression
        );

        //var iLikeExpression = Expression.Call(
        //    typeof(NpgsqlDbFunctionsExtensions),
        //    nameof(NpgsqlDbFunctionsExtensions.ILike),
        //    Type.EmptyTypes,
        //    propertyExpression,
        //    searchTextExpression
        //);

        return iLikeExpression;
    }

    protected Expression BindStartsWith_FirstTry(
        SingleValueFunctionCallNode node,
        QueryBinderContext context)
    {
        // Check that the function is startsWith
        if (node.Name != "startswith" || node.Parameters.Count() != 2)
        {
            throw new NotSupportedException($"Unsupported function '{node.Name}'");
        }

        // Extract arguments from the OData query
        var propertyNode = node.Parameters.ElementAt(0) as SingleValuePropertyAccessNode;
        var constantNode = node.Parameters.ElementAt(1) as ConstantNode;

        if (propertyNode == null || constantNode == null)
        {
            throw new InvalidOperationException("Invalid startsWith expression");
        }

        // Get the property access expression (e.g., Name)
        var propertyExpression = Bind(propertyNode, context);

        // Get the search text and append the wildcard %
        var searchText = constantNode.Value as string;
        if (searchText == null)
        {
            throw new InvalidOperationException("Invalid search text in startsWith");
        }

        // Create the expression for ILIKE using Postgres syntax
        var iLikePattern = Expression.Constant($"{searchText}%");

        // Method info for String.StartsWith
        var iLikeMethod = typeof(DbFunctionsExtensions).GetMethod(
            nameof(DbFunctionsExtensions.Like),
            new[] { typeof(DbFunctions), typeof(string), typeof(string) }
        );

        // Combine the property and the search pattern with ILIKE
        var iLikeExpression = Expression.Call(
            null,
            iLikeMethod,
            Expression.Constant(EF.Functions), // EF.Functions for SQL translation
            propertyExpression,
            iLikePattern
        );

        return iLikeExpression;

        //// Adding the collation part as a custom expression (using raw SQL if needed)
        //var collationExpression = Expression.Call(
        //    typeof(DbFunctionsExtensions).GetMethod(
        //        "Collate",
        //        BindingFlags.Static | BindingFlags.Public
        //    ),
        //    iLikeExpression,
        //    Expression.Constant(Collation)
        //);

        //return collationExpression;
    }
}