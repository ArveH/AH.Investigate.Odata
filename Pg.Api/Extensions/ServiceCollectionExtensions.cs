namespace Pg.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAllServices(
        this IServiceCollection services)
    {
        services.AddSingleton<IDbFactory, DbFactory>();
        services.AddSingleton<IODataService, ODataService>();
        services.AddControllers()
            .AddOData(opt =>
            {
                opt.AddRouteComponents(
                        "odata",
                        EdmBuilder.CreateEdmModel(),
                        s => s.AddSingleton<IFilterBinder, CustomFilterBinder>())
                    .Select().Filter();
            });

        return services;
    }
}