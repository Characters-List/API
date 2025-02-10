using System.ComponentModel.DataAnnotations;

namespace CharactersList.Models.Dto;

public class CharacterCreationDto
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = null!;
    [StringLength(24, MinimumLength = 24)]
    public string ClassId { get; set; }
    public DateTime DateOfBirth { get; set; }
}