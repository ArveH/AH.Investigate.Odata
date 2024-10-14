using Microsoft.AspNetCore.OData.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Pg.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers()
//    .AddOData(opt => opt.AddRouteComponents(
//        "odata", 
//        GetEdmModel(), 
//        s => s.AddSingleton<IFilterBinder, CustomFilterBinder>())
//        .Select().Filter());
//builder.Services.AddSingleton<IQuerySqlGeneratorFactory, CustomQuerySqlGeneratorFactory>();
builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents(
        "odata",
        GetEdmModel(),
        s => s.AddSingleton<IQuerySqlGeneratorFactory, CustomQuerySqlGeneratorFactory>())
        .Select().Filter());

//builder.Services.AddControllers()
//    .AddOData(opt => opt.AddRouteComponents(
//            "odata", 
//            GetEdmModel())
//        .Select().Filter());

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ClientContext>(
    ctx =>
    {
        ctx.UseNpgsql(connectionString);
        ctx.ReplaceService<IQuerySqlGeneratorFactory, CustomQuerySqlGeneratorFactory>();
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