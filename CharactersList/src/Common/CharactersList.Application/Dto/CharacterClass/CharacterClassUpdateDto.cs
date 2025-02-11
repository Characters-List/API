using System.ComponentModel.DataAnnotations;

namespace CharactersList.Application.Dto.CharacterClass;

public class CharacterClassUpdateDto
{
    [StringLength(50, MinimumLength = 2)]
    public string? Name { get; set; }

    [StringLength(200, MinimumLength = 10)]
    public string? Description { get; set; }
    
    [Range(5, 20)]
    public int? MaxHealth { get; set; }
}