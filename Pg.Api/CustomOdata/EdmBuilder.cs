namespace Pg.Api.CustomOdata;

public static class EdmBuilder
{
    public static IEdmModel GetEdmModelFromDbContext<TContext>() where TContext : DbContext
    {
        var builder = new ODataConventionModelBuilder();

        var dbSetProperties = typeof(TContext).GetProperties()
            .Where(p => p.PropertyType.IsGenericType && 
                        p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

        foreach (var dbSetProperty in dbSetProperties)
        {
            var entityType = dbSetProperty.PropertyType.GetGenericArguments()[0];

            var entitySetMethod = builder
                .GetType()
                .GetMethod(
                    nameof(ODataConventionModelBuilder.EntitySet), 
                    BindingFlags.Instance | BindingFlags.Public)
                ?? throw new InvalidOperationException("EntitySet MethodInfo was null");

            entitySetMethod
                .MakeGenericMethod(entityType)
                .Invoke(builder, [dbSetProperty.Name]);
        }

        return builder.GetEdmModel();
    }
}