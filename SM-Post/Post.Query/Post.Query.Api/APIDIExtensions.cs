namespace Post.Query.Api;

public static class APIDIExtensions
{
    public static IServiceCollection AddApiDI(this IServiceCollection services)
    {
        services
            .AddControllers();

        services.AddOpenApi();

        return services;
    }
}
