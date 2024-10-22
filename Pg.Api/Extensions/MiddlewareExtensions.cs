namespace Pg.Api.Extensions;

public static class MiddlewareExtensions
{
    public static WebApplication SetupMiddlewarePipeline(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}