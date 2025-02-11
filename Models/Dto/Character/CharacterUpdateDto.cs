using System.ComponentModel.DataAnnotations;

namespace CharactersList.Models.Dto.Character;

public class CharacterUpdateDto
{
    [StringLength(50, MinimumLength = 2)]
    public string? Name { get; set; }
    
    [StringLength(24, MinimumLength = 24)]
    public string? ClassId { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
}