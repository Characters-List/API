using System.ComponentModel.DataAnnotations;

namespace CharactersList.Models.Dto;

public class CharacterCreationDto
{
    public string Name { get; set; } = null!;
    [StringLength(24, MinimumLength = 24)]
    public string ClassId { get; set; }
}