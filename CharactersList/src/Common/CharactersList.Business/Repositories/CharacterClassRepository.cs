using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CharactersList.Business.Interfaces;
using CharactersList.Domain.Entities;
using CharactersList.Infrastructure.Persistence.MongoDB.Interfaces;
using MongoDB.Driver;

namespace CharactersList.Business.Repositories;

public class CharacterClassRepository: ICharacterClassRepository
{
    private readonly IEntityContext<CharacterClass> _context;

    public CharacterClassRepository(IEntityContext<CharacterClass> context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public Task<List<CharacterClass>> GetCharacterClassesAsync()
    {
        return _context.Collection.Find(_ => true).ToListAsync();
    }

    public Task<CharacterClass?> GetCharacterClassAsync(string id)
    {
        return GetCharacterClassAsync(charClass => charClass.Id == id);
    }

    public async Task<CharacterClass?> GetCharacterClassAsync(Expression<Func<CharacterClass, bool>> filter)
    {
        return await _context.Collection.Find(filter).FirstOrDefaultAsync();
    }

    public Task<CharacterClass> CreateCharacterClassAsync(CharacterClass characterClass)
    {
        return _context.Collection.InsertOneAsync(characterClass).ContinueWith(_ => characterClass);
    }

    public Task UpdateCharacterClassAsync(string id, CharacterClass characterClass)
    {
        return _context.Collection.ReplaceOneAsync(charClass => charClass.Id == id, characterClass);
    }

    public Task DeleteCharacterClassAsync(string id)
    {
        return _context.Collection.DeleteOneAsync(characterClass => characterClass.Id == id);
    }

    public Task DeleteCharacterClassesAsync(Expression<Func<CharacterClass, bool>> filter)
    {
        return _context.Collection.DeleteManyAsync(filter);
    }
}