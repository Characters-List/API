namespace CharactersList.Models.Database;

public class CharacterClass: DatabaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int MaxHealth { get; set; } = 0;
}