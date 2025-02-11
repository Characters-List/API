using System.ComponentModel.DataAnnotations;

namespace CharactersList.Models.Dto.CharacterClass;

public class CharacterClassCreationDto
{
    [StringLength(50, MinimumLength = 2)]
    public required string Name { get; set; }
    [StringLength(200, MinimumLength = 10)]
    public required string Description { get; set; }
    [Range(5, 20)]
    public int MaxHealth { get; set; }
}