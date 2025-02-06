using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CharactersList.Models;

public class Character: DatabaseEntity
{
    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be at least 3 characters long and no more than 100 characters long.")]
    public string Name { get; set; } = null!;
    [Required]
    public CharacterClass? Class { get; set; } = null!;

    public decimal Gold { get; set; } = 0;
    public int Experience { get; set; } = 0;
    public int CurrentHealth { get; set; } = 20;
    [JsonIgnore]
    public string? UserId { get; set; }
}