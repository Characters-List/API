using MongoDB.Entities;

namespace CharactersList.Models.Database;

public class Character: Entity
{
    public string Name { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public decimal Gold { get; set; } = 0;
    public int Experience { get; set; } = 0;
    public int CurrentHealth { get; set; }
    public DateTime DateOfBirth { get; set; }
    public One<CharacterClass> Class { get; set; } = null!;
}