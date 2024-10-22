var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var iLikeStrategy = LikeStrategy.CustomFilterBinder;

builder.Services.AddControllers()
    .AddOData(opt =>
    {
        opt.AddRouteComponents(
                "odata",
                EdmBuilder.CreateEdmModel(),
                s => ODataReplaceServices(ref s))
            .Select().Filter();
    });

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ClientContext>(
    ctx =>
    {
        ctx.UseNpgsql(connectionString);
        DbContextReplaceServices(ref ctx);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

void ODataReplaceServices(ref IServiceCollection serviceCollection)
{
    if (iLikeStrategy == LikeStrategy.CustomFilterBinder)
    {
        serviceCollection.AddSingleton<IFilterBinder, CustomFilterBinder>();
    }
}

void DbContextReplaceServices(ref DbContextOptionsBuilder ctx)
{
    if (iLikeStrategy == LikeStrategy.CustomQuerySqlGenerator)
    {
        ctx.ReplaceService<IQuerySqlGeneratorFactory, CustomQuerySqlGeneratorFactory>();
    }
}
