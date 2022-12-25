using NbuClient;

namespace CurrencyRateBattleServer.Infrastructure;

public static class ApplicationServiceExtension
{
    public static IServiceCollection ConfigureClients(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var uriConstrains = configuration.GetSection("NbuApiClient:ApiUrlConstrains").Get<ApiUrlConstrains>();
        
        serviceCollection.AddHttpClient<INbuApiClient, NbuApiApiClient>("NbuApiClient", config =>
        {
            config.BaseAddress = new Uri(uriConstrains.NbuApi);
        });

        serviceCollection.AddScoped<NbuApiApiClient>(service =>
        {
            var factory = service.GetService<IHttpClientFactory>();
            var httpClient = factory.CreateClient("NbuApiClient");
            return new NbuApiApiClient(httpClient);
        });

        return serviceCollection;
    }
}
