using Microsoft.AspNetCore.OData.Query.Validator;
using Microsoft.OData;

namespace Pg.Api.CustomOdata;

public class ODataService : IODataService
{
    public IQueryable<TEntity> ApplyTo<TEntity>(
        IQueryable<TEntity> query,
        HttpRequest httpRequest)
    {
        var edmModel = EdmBuilder.CreateEdmModel();
        var odataContext = new ODataQueryContext(
            edmModel,
            typeof(TEntity),
            null);

        var validationSettings = new ODataValidationSettings
        {
            AllowedQueryOptions = AllowedQueryOptions.All
        };
        var odataQueryOptions = new ODataQueryOptions<TEntity>(
            odataContext,
            httpRequest);

        ValidateQueryOptions(odataQueryOptions, validationSettings);

        var filteredEntities = (IQueryable<TEntity>)odataQueryOptions.ApplyTo(query);
        return filteredEntities;
    }

    private static void ValidateQueryOptions<TEntity>(ODataQueryOptions<TEntity> odataQueryOptions,
        ODataValidationSettings validationSettings)
    {
        try
        {
            odataQueryOptions.Validate(validationSettings);
        }
        catch (ODataException ex)
        {
            throw new ODataException(ex.Message);
        }
    }

}