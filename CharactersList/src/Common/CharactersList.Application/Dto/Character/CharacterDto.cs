using System;
using CharactersList.Application.Dto.CharacterClass;
using CharactersList.Models.Dto;

namespace CharactersList.Application.Dto.Character;

public class CharacterDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required MinimalCharacterClassDto Class { get; set; }
    public required decimal Gold { get; set; }
    public required int Experience { get; set; }
    public required HealthDto Health { get; set; }
    public required string UserId { get; set; }
    public required DateTime DateOfBirth { get; set; }
}