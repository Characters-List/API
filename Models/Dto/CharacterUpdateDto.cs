namespace CharactersList.Models.Dto;

public class CharacterUpdateDto
{
    public string? Name { get; set; } = null!;
    public decimal? Gold { get; set; } = 0;
    public int? Experience { get; set; } = 0;
    public int? CurrentHealth { get; set; } = 0;
    public int? MaxHealth { get; set; } = 0;
}