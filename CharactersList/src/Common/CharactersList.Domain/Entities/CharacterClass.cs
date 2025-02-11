namespace CharactersList.Domain.Entities;

public class CharacterClass: Entity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int MaxHealth { get; set; } = 0;
}