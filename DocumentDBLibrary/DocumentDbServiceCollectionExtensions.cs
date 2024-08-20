using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

public static class DocumentDbServiceCollectionExtensions
{
    public static IServiceCollection AddDocumentDbServices(this IServiceCollection services, string connectionString, string databaseName)
    {
        services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(connectionString));
        services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(databaseName));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(ITextSearchRepository<>), typeof(TextSearchRepository<>));

        return services;
    }
}
