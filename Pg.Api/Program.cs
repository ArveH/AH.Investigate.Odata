using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Pg.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#if true // Add service for CustomFilterBinder
builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents(
        "odata",
        GetEdmModel(),
        s => s.AddSingleton<IFilterBinder, CustomFilterBinder>())
        .Select().Filter());
#else // No added services
builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents(
            "odata",
            GetEdmModel())
        .Select().Filter());
#endif

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ClientContext>(
    ctx =>
    {
        ctx.UseNpgsql(connectionString);
#if false
        ctx.ReplaceService<IQuerySqlGeneratorFactory, CustomQuerySqlGeneratorFactory>();
#endif
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