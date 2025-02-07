namespace CharactersList.Models.Dto;

public class CharacterClassDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int MaxHealth { get; set; }
}