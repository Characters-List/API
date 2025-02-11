using CharactersList.Domain.Entities;
using MongoDB.Driver;

namespace CharactersList.Infrastructure.Persistence.MongoDB.Interfaces;

public interface IEntityContext<T> where T: Entity
{
    IMongoCollection<T> Collection { get; }
}