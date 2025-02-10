using CharactersList.Models.Database;

namespace CharactersList.Models.Dto;

public class CharacterClassDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public static CharacterClassDto FromCharacterClass(CharacterClass characterClass)
    {
        return new CharacterClassDto
        {
            Id = characterClass.ID,
            Name = characterClass.Name,
            Description = characterClass.Description
        };
    }
}