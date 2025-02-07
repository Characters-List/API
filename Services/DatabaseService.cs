using System.Linq.Expressions;
using System.Reflection;
using CharactersList.Configuration.Database;
using CharactersList.Models.Database;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CharactersList.Services;

public class DatabaseService<T> where T : DatabaseEntity
{
    IOptions<CharactersListDatabaseSettings> _databaseSettings;
    private readonly IMongoCollection<T> _documents;
    
    public DatabaseService(IOptions<CharactersListDatabaseSettings> databaseSettings)
    {
        MongoClient client = new MongoClient(databaseSettings.Value.ConnectionString);
        
        IMongoDatabase _db = client.GetDatabase(databaseSettings.Value.DatabaseName);
        _documents = _db.GetCollection<T>(typeof(T).Name);
        _databaseSettings = databaseSettings;
    }
    
    private IFindFluent<T, T> Find(Expression<Func<T, bool>> predicate)
    {
        return _documents.Find(predicate);
    }
    
    public async Task<List<T>> Get(Expression<Func<T, bool>> predicate)
    {
        return await Find(predicate).ToListAsync();
    }

    public async Task<List<T>> Get()
    {
        return await Get(_ => true);
    }
    
    public async Task<T?> GetUnique(string id)
    {
        return await GetUnique(d => d.Id == id);
    }
    
    public async Task<T?> GetUnique(Expression<Func<T, bool>> predicate)
    {
        return await Find(predicate).FirstOrDefaultAsync();
    }
    
    public async Task<T> Create(T document)
    {
        await _documents.InsertOneAsync(document);
        
        return document;
    }
    
    public async Task Update(string id, T document)
    {
        await _documents.ReplaceOneAsync(d => d.Id == id, document);
    }
    
    public async Task Delete(string id)
    {
        await _documents.DeleteOneAsync(d => d.Id == id);
    }
}