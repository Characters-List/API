using Asp.Versioning;
using CharactersList.Application.Dto.Character;
using CharactersList.Application.Dto.CharacterClass;
using CharactersList.Business.Interfaces;
using CharactersList.Domain.Entities;
using CharactersList.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharactersList.Api.Controllers;

[ApiController]
[Authorize(Policy = "user")]
[Route("/api/v{v:apiVersion}/[controller]")]
[ApiVersion(1)]
public class CharacterClassesController: ControllerBase
{
    private readonly ICharacterClassRepository _characterClassRepository;
    private readonly ICharacterRepository _characterDatabaseService;
    
    public CharacterClassesController(ICharacterClassRepository characterClassRepository, ICharacterRepository characterDatabaseService)
    {
        _characterClassRepository = characterClassRepository;
        _characterDatabaseService = characterDatabaseService;
    }
    
    [HttpGet]
    public async Task<List<CharacterClassDto>> Get()
    {
        IEnumerable<CharacterClass> classes = await _characterClassRepository.GetCharacterClassesAsync();

        return classes.Select(
            charClass => new CharacterClassDto 
            {
                Id = charClass.Id,
                Name = charClass.Name,
                Description = charClass.Description,
                MaxHealth = charClass.MaxHealth
            }
        ).ToList();
    }
    
    [HttpGet("{id:length(24)}")]
    public async Task<CharacterClassDto?> Get(string id)
    {
        CharacterClass? characterClass = await _characterClassRepository.GetCharacterClassAsync(id);
        
        if (characterClass is null)
        {
            return null;
        }

        return new CharacterClassDto
        {
            Id = characterClass.Id,
            Name = characterClass.Name,
            Description = characterClass.Description,
            MaxHealth = characterClass.MaxHealth
        };
    }
    
    [HttpGet("{id:length(24)}/Characters")]
    public async Task<List<CharacterDto>> GetCharactersWithClassId(string id)
    {
        CharacterClass? characterClass = await _characterClassRepository.GetCharacterClassAsync(id);
        
        if (characterClass is null)
        {
            return [];
        }

        IEnumerable<Character> characters = await _characterDatabaseService.GetCharactersAsync();

        return
            characters.Select(
                character => new CharacterDto
                {
                    Class = new MinimalCharacterClassDto
                    {
                        Id = characterClass.Id,
                        Name = characterClass.Name,
                    },
                    Id = character.Id,
                    Name = character.Name,
                    Health = new HealthDto
                    {
                        Current = character.CurrentHealth,
                        Max = characterClass.MaxHealth
                    },
                    UserId = character.UserId,
                    DateOfBirth = character.DateOfBirth,
                    Experience = character.Experience,
                    Gold = character.Gold
                }
            ).ToList();
    }
    
    [HttpPost]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> Create(CharacterClassCreationDto characterClass)
    {
        CharacterClass createdClass = await _characterClassRepository.CreateCharacterClassAsync(
            new CharacterClass
            {
                Name = characterClass.Name,
                Description = characterClass.Description,
                MaxHealth = characterClass.MaxHealth
            }
        );
        
        return CreatedAtAction(nameof(Get), new { id = createdClass.Id }, createdClass);
    }

    [HttpPut("{id:length(24)}")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> Update(string id, CharacterClassUpdateDto update)
    {
        CharacterClass? existingCharacterClass = await _characterClassRepository.GetCharacterClassAsync(id);
        
        if (existingCharacterClass is null)
        {
            return NotFound();
        }
        
        await _characterClassRepository.UpdateCharacterClassAsync(
            id,
            new CharacterClass
            {
                Id = id,
                Name = update.Name ?? existingCharacterClass.Name,
                Description = update.Description ?? existingCharacterClass.Description,
                MaxHealth = update.MaxHealth ?? existingCharacterClass.MaxHealth
            }
        );
        
        return NoContent();
    }
    
    [HttpDelete("{id:length(24)}")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> Delete(string id)
    {
        CharacterClass? characterClass = await _characterClassRepository.GetCharacterClassAsync(id);
        
        if (characterClass is null)
        {
            return NotFound();
        }
        
        await _characterDatabaseService.DeleteCharactersAsync(character => character.ClassId == id);
        await _characterClassRepository.DeleteCharacterClassAsync(id);
        
        return NoContent();
    }
}