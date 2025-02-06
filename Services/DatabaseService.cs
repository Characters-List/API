using System.Linq.Expressions;
using CharactersList.Configuration.Database;
using CharactersList.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CharactersList.Services;

public class DatabaseService<T> where T : DatabaseEntity
{
    private readonly IMongoCollection<T> _documents;
    
    public DatabaseService(IOptions<CharactersListDatabaseSettings> databaseSettings)
    {
        MongoClient client = new MongoClient(databaseSettings.Value.ConnectionString);
        IMongoDatabase? db = client.GetDatabase(databaseSettings.Value.DatabaseName);
        _documents = db.GetCollection<T>(typeof(T).Name);
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
        return await Find(_ => true).ToListAsync();
    }
    
    public async Task<T?> Get(string id)
    {
        return await Find(document => document.Id == id).FirstOrDefaultAsync();
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