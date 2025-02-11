using CharactersList.Models.Database;

namespace CharactersList.Models.Dto.CharacterClass;

public class CharacterClassDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int MaxHealth { get; set; } = 0;

    public static CharacterClassDto FromCharacterClass(Database.CharacterClass characterClass)
    {
        return new CharacterClassDto
        {
            Id = characterClass.ID,
            Name = characterClass.Name,
            Description = characterClass.Description,
            MaxHealth = characterClass.MaxHealth
        };
    }
}