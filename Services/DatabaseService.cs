using System.Linq.Expressions;
using System.Reflection;
using CharactersList.Configuration.Database;
using CharactersList.Models.Database;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Entities;

namespace CharactersList.Services;

public class DatabaseService<T> where T : Entity
{
    public static bool IsInitialized { get; private set; } = false;
    
    public DatabaseService(IOptions<CharactersListDatabaseSettings> databaseSettings)
    {
        Init(databaseSettings);
    }

    public static void Init(IOptions<CharactersListDatabaseSettings> databaseSettings)
    {
        if (IsInitialized)
        {
            return;
        }
        
        Task
            .Run(async () =>
                await DB.InitAsync(
                    databaseSettings.Value.DatabaseName,
                    MongoClientSettings.FromConnectionString(
                        databaseSettings.Value.ConnectionString
                    )
                )
            )
            .GetAwaiter()
            .GetResult();
        
        IsInitialized = true;
    }
    
    private Find<T, T> Find(Expression<Func<T, bool>> predicate)
    {
        return DB.Find<T>().Match(predicate);
    }
    
    public async Task<List<T>> Get(Expression<Func<T, bool>> predicate)
    {
        return await Find(predicate).ExecuteAsync();
    }

    public async Task<List<T>> Get()
    {
        return await Get(_ => true);
    }
    
    public async Task<T?> GetUnique(string id)
    {
        return await DB.Find<T>().MatchID(id).ExecuteFirstAsync();
    }
    
    public async Task<T?> GetUnique(Expression<Func<T, bool>> predicate)
    {
        return await Find(predicate).ExecuteFirstAsync();
    }
    
    public async Task<T> Create(T document)
    {
        await document.SaveAsync();
        
        return document;
    }
    
    public async Task<bool> Update(string id, T document)
    {
        UpdateResult result = await DB.Update<T>()
            .MatchID(id)
            .ModifyWith(document)
            .ExecuteAsync();

        return result is { IsAcknowledged: true, ModifiedCount: > 0 };
    }
    
    public async Task Delete(string id)
    {
        await DB.DeleteAsync<T>(id);
    }
}