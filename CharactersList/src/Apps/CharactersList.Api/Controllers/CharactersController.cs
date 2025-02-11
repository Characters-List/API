using System.Security.Claims;
using CharactersList.Application.Dto.Character;
using CharactersList.Application.Dto.CharacterClass;
using CharactersList.Business.Interfaces;
using CharactersList.Domain.Entities;
using CharactersList.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharactersList.Api.Controllers;

[Authorize(Policy = "user")]
[ApiController]
[Route("/api/v1/[controller]")]
public class CharactersController: ControllerBase
{
    private readonly ICharacterClassRepository _characterClassRepository;
    private readonly ICharacterRepository _characterDatabaseService;
    
    public CharactersController(
        ICharacterClassRepository characterClassRepository,
        ICharacterRepository characterDatabaseService
    )
    {
        _characterClassRepository = characterClassRepository;
        _characterDatabaseService = characterDatabaseService;
    }
    
    [HttpGet]
    public async Task<List<CharacterDto>> Get()
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        List<Character> characters = await _characterDatabaseService.GetCharactersAsync(character => character.UserId == userId);
        
        CharacterDto?[] dtos = await Task.WhenAll(
            characters.Select(
                async Task<CharacterDto?> (character) =>
                {
                    CharacterClass? characterClass = await _characterClassRepository.GetCharacterClassAsync(character.ClassId);
                    
                    if (characterClass is null)
                    {
                        return null;
                    }
                    
                    return new CharacterDto
                    {
                        Id = character.Id,
                        Name = character.Name,
                        Class = new MinimalCharacterClassDto
                        {
                            Id = characterClass.Id,
                            Name = characterClass.Name,
                        },
                        Health = new HealthDto
                        {
                            Current = character.CurrentHealth,
                            Max = characterClass.MaxHealth
                        },
                        DateOfBirth = character.DateOfBirth,
                        Gold = character.Gold,
                        Experience = character.Experience,
                        UserId = character.UserId
                    };
                }
            )
        );
        
        return dtos.Where(dto => dto is not null).Select(dto => dto!).ToList();
    }
    
    [HttpGet("{id:length(24)}")]
    public async Task<CharacterDto?> Get(string id)
    {
        Character? character = await _characterDatabaseService.GetCharacterAsync(id);
        
        if (character is null)
        {
            return null;
        }
        
        CharacterClass? characterClass = await _characterClassRepository.GetCharacterClassAsync(character.ClassId);
        
        if (characterClass is null)
        {
            return null;
        }

        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        
        if (character.UserId != userId)
        {
            return null;
        }

        return new CharacterDto
        {
            Id = character.Id,
            Name = character.Name,
            Class = new MinimalCharacterClassDto
            {
                Id = character.ClassId,
                Name = characterClass.Name
            },
            Health = new HealthDto
            {
                Current = character.CurrentHealth,
                Max = characterClass.MaxHealth
            },
            DateOfBirth = character.DateOfBirth,
            Gold = character.Gold,
            Experience = character.Experience,
            UserId = character.UserId
        };
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CharacterCreationDto character)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userId is null)
        {
            return Unauthorized();
        }
        
        CharacterClass? characterClass = await _characterClassRepository.GetCharacterClassAsync(character.ClassId);

        if (characterClass is null)
        {
            ModelState.AddModelError(nameof(character.ClassId), "Invalid character class.");
            
            return BadRequest(ModelState);
        }

        Character createdCharacter = await _characterDatabaseService.CreateCharacterAsync(
            new Character
            {
                Name = character.Name,
                ClassId = characterClass.Id,
                CurrentHealth = characterClass.MaxHealth,
                UserId = userId,
                DateOfBirth = character.DateOfBirth,
                Gold = 0,
                Experience = 0
            }
        );
        
        return CreatedAtAction(
            nameof(Get),
            new { id = createdCharacter.Id },
            new CharacterDto
            {
                Id = createdCharacter.Id,
                Name = createdCharacter.Name,
                Class = new MinimalCharacterClassDto
                {
                    Id = characterClass.Id,
                    Name = characterClass.Name
                },
                Health = new HealthDto
                {
                    Current = createdCharacter.CurrentHealth,
                    Max = characterClass.MaxHealth
                },
                DateOfBirth = createdCharacter.DateOfBirth,
                Gold = createdCharacter.Gold,
                Experience = createdCharacter.Experience,
                UserId = createdCharacter.UserId
            }
        );
        
    }
    
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, CharacterUpdateDto update)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userId is null)
        {
            return Unauthorized();
        }

        Character? existingCharacter = await _characterDatabaseService.GetCharacterAsync(id);
        
        if (existingCharacter is null)
        {
            return NotFound();
        }
        
        if (existingCharacter.UserId != userId)
        {
            return Forbid();
        }
        
        CharacterClass? newClass = await _characterClassRepository.GetCharacterClassAsync(update.ClassId ?? existingCharacter.ClassId);
        
        if (newClass is null)
        {
            ModelState.AddModelError(nameof(update.ClassId), "Invalid character class.");
            
            return BadRequest(ModelState);
        }

        await _characterDatabaseService.UpdateCharacterAsync(
            id,
            new Character
            {
                Id = id,
                Name = update.Name ?? existingCharacter.Name,
                ClassId = newClass.Id,
                CurrentHealth = existingCharacter.CurrentHealth,
                UserId = existingCharacter.UserId,
                DateOfBirth = update.DateOfBirth ?? existingCharacter.DateOfBirth,
                Gold = existingCharacter.Gold,
                Experience = existingCharacter.Experience
            }
        );
        
        return NoContent();
    }
    
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        Character? character = await _characterDatabaseService.GetCharacterAsync(id);
        
        if (character is null)
        {
            return NotFound();
        }
        
        if (character.UserId != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
        {
            return Forbid();
        }

        await _characterDatabaseService.DeleteCharacterAsync(id);
        
        return NoContent();
    }
}
