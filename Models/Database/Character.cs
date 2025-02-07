using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CharactersList.Configuration.Database;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CharactersList.Models.Database;

public class Character: DatabaseEntity
{
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must be at least 3 characters long and no more than 20 characters long.")]
    public string Name { get; set; } = null!;
    
    [BsonIgnore]
    public CharacterClass Class { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string ClassId { get; set; } = null!;
    
    [JsonIgnore]
    public string? UserId { get; set; }
    
    public decimal Gold { get; set; } = 0;
    public int Experience { get; set; } = 0;
    public int CurrentHealth { get; set; }
    
}