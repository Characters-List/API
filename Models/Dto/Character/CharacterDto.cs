using CharactersList.Models.Dto.CharacterClass;

namespace CharactersList.Models.Dto.Character;

public class CharacterDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public CharacterClassDto Class { get; set; } = null!;
    public decimal Gold { get; set; } = 0;
    public int Experience { get; set; } = 0;
    public HealthDto Health { get; set; }
    public string UserId { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    
    public static async Task<CharacterDto> FromCharacter(Database.Character character)
    {
        Database.CharacterClass characterClass = await character.Class.ToEntityAsync();
        
        return new CharacterDto
        {
            Id = character.ID,
            Name = character.Name,
            Class = CharacterClassDto.FromCharacterClass(characterClass),
            Gold = character.Gold,
            Experience = character.Experience,
            Health = new HealthDto
            {
                Current = character.CurrentHealth,
                Max = characterClass.MaxHealth
            },
            UserId = character.UserId,
            DateOfBirth = character.DateOfBirth
        };
    }
}