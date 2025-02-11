using System.Collections;
using CharactersList.Models.Database;
using CharactersList.Models.Dto;
using CharactersList.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;
using MongoDB.Entities;

namespace CharactersList.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class CharacterClassesController: ControllerBase
{
    private readonly DatabaseService<CharacterClass> _characterClassDatabaseService;
    private readonly DatabaseService<Character> _characterDatabaseService;
    
    public CharacterClassesController(DatabaseService<CharacterClass> characterClassDatabaseService, DatabaseService<Character> characterDatabaseService)
    {
        _characterClassDatabaseService = characterClassDatabaseService;
        _characterDatabaseService = characterDatabaseService;
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
    
    [HttpGet("{id:length(24)}/Characters")]
    public async Task<CharacterDto[]> GetCharactersWithClassId(string id)
    {
        CharacterClass? characterClass = await _characterClassDatabaseService.GetUnique(id);
        
        if (characterClass is null)
        {
            return [];
        }
        
        IEnumerable<Character> characters = await _characterDatabaseService.Get(character => character.Class.ID == id);
        
        return await Task.WhenAll(characters.Select(CharacterDto.FromCharacter));
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