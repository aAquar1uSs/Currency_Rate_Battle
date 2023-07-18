using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccountManager.Redis.Infrastructure;

public static class RedisConfiguration
{
    public static IServiceCollection ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options => { options.Configuration = configuration["RedisCacheUrl"]; });
    }
}
