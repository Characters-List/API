using CharactersList.Domain.Entities;
using CharactersList.Infrastructure.Persistence.MongoDB.Interfaces;
using MongoDB.Driver;

namespace CharactersList.Infrastructure.Persistence.MongoDB.Contexts;

public class CharacterContext: IEntityContext<Character>
{
    public IMongoCollection<Character> Collection { get; }
    
    public CharacterContext(IMongoDatabase database)
    {
        Collection = database.GetCollection<Character>(nameof(Character));
    }
}