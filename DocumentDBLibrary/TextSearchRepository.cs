using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TextSearchRepository<T> : Repository<T>, ITextSearchRepository<T>
{
    private readonly IMongoDatabase _database;

    public TextSearchRepository(IMongoDatabase database, string collectionName)
        : base(database, collectionName)
    {
        _database = database;
    }

    public async Task<IEnumerable<T>> SearchAsync(string searchText)
    {
        var filter = Builders<T>.Filter.Text(searchText);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task CreateTextIndexAsync(params string[] fields)
    {
        var indexKeys = Builders<T>.IndexKeys.Text(string.Join(" ", fields));
        var indexModel = new CreateIndexModel<T>(indexKeys);

        await _collection.Indexes.CreateOneAsync(indexModel);
    }
}
