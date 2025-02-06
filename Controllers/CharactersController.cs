using System.Security.Claims;
using CharactersList.Models;
using CharactersList.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharactersList.Controllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class CharactersController: ControllerBase
{
    private DatabaseService<Character> _service;
    
    public CharactersController(DatabaseService<Character> service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<List<Character>> Get()
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        
        return await _service.Get(character => character.UserId == userId);
    }
    
    [HttpGet("{id:length(24)}")]
    public async Task<Character?> Get(string id)
    {
        Character? character = await _service.Get(id);
        
        if (character is null)
        {
            return null;
        }
        
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
        
        if (character.UserId != userId)
        {
            return null;
        }

        return character;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Character character)
    {
        if (character.Class is null)
        {
            ModelState.AddModelError("Class", "Class is required.");
            
            return BadRequest(ModelState);
        }
        
        if (!Enum.IsDefined(typeof(CharacterClass), character.Class.Value))
        {
            ModelState.AddModelError("Class", "Invalid character class.");

            return BadRequest(ModelState);
        }
        
        Character createdCharacter = await _service.Create(new Character
        {
            Name = character.Name,
            Class = character.Class,
            Gold = 0,
            Experience = 0,
            CurrentHealth = 20,
            UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        });
        
        return CreatedAtAction(nameof(Create), new { id = createdCharacter.Id }, createdCharacter);
    }
    
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Character character)
    {
        Character? existingCharacter = await _service.Get(id);
        
        if (existingCharacter is null)
        {
            return NotFound();
        }

        character.Id = id;
        
        await _service.Update(id, character);
        
        return NoContent();
    }
    
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        Character? character = await _service.Get(id);
        
        if (character is null)
        {
            return NotFound();
        }
        
        await _service.Delete(id);
        
        return NoContent();
    }
}
