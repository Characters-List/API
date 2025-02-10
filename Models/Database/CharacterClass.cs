using MongoDB.Entities;

namespace CharactersList.Models.Database;

public class CharacterClass: Entity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int MaxHealth { get; set; } = 0;
    public Many<Character, CharacterClass> Characters { get; set; } = null!;

    public CharacterClass()
    {
        this.InitOneToMany(() => Characters);
    }
}