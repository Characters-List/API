namespace CharactersList.Application.Dto.CharacterClass;

public class CharacterClassDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int MaxHealth { get; set; }
}