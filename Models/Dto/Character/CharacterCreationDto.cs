using System.ComponentModel.DataAnnotations;

namespace CharactersList.Models.Dto.Character;

public class CharacterCreationDto
{
    [StringLength(50, MinimumLength = 2)]
    public required string Name { get; set; } = null!;
    
    [StringLength(24, MinimumLength = 24)]
    public required string ClassId { get; set; }
    public required DateTime DateOfBirth { get; set; }
}