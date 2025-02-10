using CharactersList.Models.Database;

namespace CharactersList.Models.Dto;

public class CharacterDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Class { get; set; } = null!;
    public decimal Gold { get; set; } = 0;
    public int Experience { get; set; } = 0;
    public HealthDto Health { get; set; }
    
    public static CharacterDto FromCharacter(Character character)
    {
        return new CharacterDto
        {
            Id = character.ID,
            Name = character.Name,
            Class = "",
            Gold = character.Gold,
            Experience = character.Experience,
            Health = new HealthDto
            {
                Current = character.CurrentHealth,
                Max = 0
            }
        };
    }
}