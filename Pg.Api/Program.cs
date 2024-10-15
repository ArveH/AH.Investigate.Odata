var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var iLikeStrategy = LikeStrategy.CustomQuerySqlGenerator;

builder.Services.AddControllers()
    .AddOData(opt =>
    {
        opt.AddRouteComponents(
                "odata",
                GetEdmModel(),
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

app.Run();

static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<Client>("Clients");
    return builder.GetEdmModel();
}

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
