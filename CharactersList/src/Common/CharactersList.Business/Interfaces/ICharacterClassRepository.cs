using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CharactersList.Domain.Entities;

namespace CharactersList.Business.Interfaces;

public interface ICharacterClassRepository
{
    public Task<List<CharacterClass>> GetCharacterClassesAsync();
    public Task<CharacterClass?> GetCharacterClassAsync(string id);
    public Task<CharacterClass?> GetCharacterClassAsync(Expression<Func<CharacterClass, bool>> filter);
    public Task<CharacterClass> CreateCharacterClassAsync(CharacterClass characterClass);
    public Task UpdateCharacterClassAsync(string id, CharacterClass characterClass);
    public Task DeleteCharacterClassAsync(string id);
    public Task DeleteCharacterClassesAsync(Expression<Func<CharacterClass, bool>> filter);
}