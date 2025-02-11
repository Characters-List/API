using CharactersList.Models.Database;
using CharactersList.Models.Dto.Character;
using CharactersList.Models.Dto.CharacterClass;
using CharactersList.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CharactersList.Controllers;

[ApiController]
[Authorize]
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
        List<string> permissions = User.Claims.Where(claim => claim.Type == "permissions").Select(claim => claim.Value).ToList();
        if (!permissions.Contains("admin"))
        {
            return Unauthorized();
        }
        
        CharacterClass createdClass = await _characterClassDatabaseService.Create(new CharacterClass
        {
            Name = characterClass.Name,
            Description = characterClass.Description,
            MaxHealth = characterClass.MaxHealth
        });
        
        return CreatedAtAction(nameof(Get), new { id = createdClass.ID }, createdClass);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, CharacterClassUpdateDto update)
    {
        List<string> permissions = User.Claims.Where(claim => claim.Type == "permissions").Select(claim => claim.Value).ToList();
        if (!permissions.Contains("admin"))
        {
            return Unauthorized();
        }
        
        CharacterClass? existingCharacterClass = await _characterClassDatabaseService.GetUnique(id);
        
        if (existingCharacterClass is null)
        {
            return NotFound();
        }
        
        bool result = await _characterClassDatabaseService.Update(
            id,
            new CharacterClass
            {
                ID = id,
                Name = update.Name ?? existingCharacterClass.Name,
                Description = update.Description ?? existingCharacterClass.Description,
                MaxHealth = update.MaxHealth ?? existingCharacterClass.MaxHealth
            }
        );
        
        return result ? NoContent() : NotFound();
    }
    
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        CharacterClass? characterClass = await _characterClassDatabaseService.GetUnique(id);
        
        if (characterClass is null)
        {
            return NotFound();
        }
        
        // NOTE: Deleting the class won't delete the characters, so we need to delete them manually 
        await _characterDatabaseService.Delete(character => character.Class.ID == id);
        await _characterClassDatabaseService.Delete(id);
        
        return NoContent();
    }
}