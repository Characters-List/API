using System.Linq.Expressions;
using CharactersList.Domain.Entities;

namespace CharactersList.Business.Interfaces;

public interface ICharacterRepository
{
    Task<List<Character>> GetCharactersAsync();
    Task<List<Character>> GetCharactersAsync(Expression<Func<Character, bool>> filter);
    Task<Character?> GetCharacterAsync(string id);
    Task<Character> CreateCharacterAsync(Character character);
    Task UpdateCharacterAsync(string id, Character characterIn);
    Task DeleteCharacterAsync(string id);
    Task DeleteCharactersAsync(Expression<Func<Character, bool>> filter);
}