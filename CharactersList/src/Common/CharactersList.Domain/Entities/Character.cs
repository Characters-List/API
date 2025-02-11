using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CharactersList.Domain.Entities;

public class Character: Entity
{
    public required string Name { get; set; }
    public required string UserId { get; set; }
    public required decimal Gold { get; set; }
    public required int Experience { get; set; }
    public required int CurrentHealth { get; set; }
    public required DateTime DateOfBirth { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public required string ClassId { get; set; }
}