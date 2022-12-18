using NbuClient;

namespace CurrencyRateBattleServer.Infrastructure;

public static class ApplicationServiceExtension
{
    public static IServiceCollection ConfigureClients(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var uriConstrains = configuration.GetSection("NbuApiClient").GetValue<ApiUrlConstrains>("Uri");
        
        serviceCollection.AddHttpClient<NbuApiClient>("NbuApiClient", config =>
        {
            config.BaseAddress = new Uri(uriConstrains.NbuApi);
        });

        serviceCollection.AddScoped<NbuApiClient>(service =>
        {
            var factory = service.GetService<IHttpClientFactory>();
            var httpClient = factory.CreateClient("NbuApiClient");
            return new NbuApiClient(httpClient);
        });

        return serviceCollection;
    }
}
