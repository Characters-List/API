using CharactersList.Domain.Entities;
using CharactersList.Infrastructure.Persistence.MongoDB.Interfaces;
using MongoDB.Driver;

namespace CharactersList.Infrastructure.Persistence.MongoDB.Contexts;

public class CharacterClassContext: IEntityContext<CharacterClass>
{
    public IMongoCollection<CharacterClass> Collection { get; }

    public CharacterClassContext(IMongoDatabase database)
    {
        Collection = database.GetCollection<CharacterClass>(nameof(CharacterClass));
    }
}