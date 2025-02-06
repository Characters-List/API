using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CharactersList.Models;

public abstract class DatabaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
}
