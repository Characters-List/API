using System.Collections;
using System.Security.Claims;
using CharactersList.Models.Database;
using CharactersList.Models.Dto;
using CharactersList.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace CharactersList.Controllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class CharactersController: ControllerBase
{
    private DatabaseService<Character> _characterDatabaseService;
    private DatabaseService<CharacterClass> _characterClassDatabaseService;
    
    public CharactersController(
        DatabaseService<Character> characterDatabaseService,
        DatabaseService<CharacterClass> characterClassDatabaseService
    )
    {
        _characterDatabaseService = characterDatabaseService;
        _characterClassDatabaseService = characterClassDatabaseService;
    }
    
    [HttpGet]
    public async Task<CharacterDto[]> Get()
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        List<Character> characters = await _characterDatabaseService.Get(character => character.UserId == userId);
        
        return await Task.WhenAll(characters.Select(CharacterDto.FromCharacter));
    }
    
    [HttpGet("{id:length(24)}")]
    public async Task<CharacterDto?> Get(string id)
    {
        Character? character = await _characterDatabaseService.GetUnique(id);
        
        if (character is null)
        {
            return null;
        }
        
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        
        if (character.UserId != userId)
        {
            return null;
        }

        return await CharacterDto.FromCharacter(character);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CharacterCreationDto character)
    {
        CharacterClass? characterClass = await _characterClassDatabaseService.GetUnique(character.ClassId);

        if (characterClass is null)
        {
            ModelState.AddModelError(nameof(character.ClassId), "Invalid character class.");
            
            return BadRequest(ModelState);
        }

        Character createdCharacter = await _characterDatabaseService.Create(
            new Character
            {
                Name = character.Name,
                Class = characterClass.ToReference(),
                CurrentHealth = characterClass.MaxHealth,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                DateOfBirth = character.DateOfBirth,
            }
        );
        
        return CreatedAtAction(
            nameof(Get),
            new { id = createdCharacter.ID },
            await CharacterDto.FromCharacter(createdCharacter)
        );
        
    }
    
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, CharacterUpdateDto update)
    {
        Character? existingCharacter = await _characterDatabaseService.GetUnique(id);
        
        if (existingCharacter is null)
        {
            return NotFound();
        }
        
        if (existingCharacter.UserId != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
        {
            return Forbid();
        }
        
        if (update.CurrentHealth is < 0)
        {
            ModelState.AddModelError("CurrentHealth", "Current health cannot be negative.");
            
            return BadRequest(ModelState);
        }

        CharacterClass characterClass = await existingCharacter.Class.ToEntityAsync();
        
        if (update.CurrentHealth != null && update.CurrentHealth > characterClass.MaxHealth)
        {
            ModelState.AddModelError("CurrentHealth", "Current health cannot be greater than max health.");
            
            return BadRequest(ModelState);
        }

        bool result = await _characterDatabaseService.Update(
            id,
            new Character
            {
                ID = id,
                Name = update.Name ?? existingCharacter.Name,
                Class = existingCharacter.Class,
                CurrentHealth = update.CurrentHealth ?? existingCharacter.CurrentHealth,
                UserId = existingCharacter.UserId
            }
        );
        
        return result ? NoContent() : NotFound();
    }
    
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        Character? character = await _characterDatabaseService.GetUnique(id);
        
        if (character is null)
        {
            return NotFound();
        }
        
        if (character.UserId != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
        {
            return Forbid();
        }
        
        await _characterDatabaseService.Delete(id);
        
        return NoContent();
    }
}
