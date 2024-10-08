var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel()).Select().Filter());

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ClientContext>(
    ctx => ctx.UseNpgsql(connectionString));

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