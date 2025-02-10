using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CharactersList.Configuration.Database;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace CharactersList.Models.Database;

public class Character: Entity
{
    public string Name { get; set; } = null!;
    public string? UserId { get; set; }
    public decimal Gold { get; set; } = 0;
    public int Experience { get; set; } = 0;
    public int CurrentHealth { get; set; }
    public One<CharacterClass> Class { get; set; } = null!;
}