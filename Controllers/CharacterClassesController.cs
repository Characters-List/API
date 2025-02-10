using CharactersList.Models.Database;
using CharactersList.Models.Dto;
using CharactersList.Services;
using Microsoft.AspNetCore.Mvc;

namespace CharactersList.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class CharacterClassesController: ControllerBase
{
    private DatabaseService<CharacterClass> _characterClassDatabaseService;
    
    public CharacterClassesController(DatabaseService<CharacterClass> characterClassDatabaseService)
    {
        _characterClassDatabaseService = characterClassDatabaseService;
    }
    
    [HttpGet]
    public async Task<List<CharacterClassDto>> Get()
    {
        List<CharacterClass> classes = await _characterClassDatabaseService.Get();

        return classes.Select(CharacterClassDto.FromCharacterClass).ToList();
    }
    
    [HttpGet("{id:length(24)}")]
    public async Task<CharacterClassDto?> Get(string id)
    {
        CharacterClass? characterClass = await _characterClassDatabaseService.GetUnique(id);

        return characterClass is not null ? CharacterClassDto.FromCharacterClass(characterClass) : null;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CharacterClassCreationDto characterClass)
    {
        CharacterClass createdClass = await _characterClassDatabaseService.Create(new CharacterClass
        {
            Name = characterClass.Name,
            Description = characterClass.Description,
            MaxHealth = characterClass.MaxHealth
        });
        
        return CreatedAtAction(nameof(Get), new { id = createdClass.ID }, createdClass);
    }
    
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        CharacterClass? characterClass = await _characterClassDatabaseService.GetUnique(id);
        
        if (characterClass is null)
        {
            return NotFound();
        }
        
        await _characterClassDatabaseService.Delete(id);
        
        return NoContent();
    }
}