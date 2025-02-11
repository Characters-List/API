using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CharactersList.Business.Interfaces;
using CharactersList.Domain.Entities;
using CharactersList.Infrastructure.Persistence.MongoDB.Interfaces;
using MongoDB.Driver;

namespace CharactersList.Business.Repositories;

public class CharacterRepository: ICharacterRepository
{
    private readonly IEntityContext<Character> _context;
    
    public CharacterRepository(IEntityContext<Character> context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public Task<List<Character>> GetCharactersAsync()
    {
        return GetCharactersAsync(_ => true);
    }

    public Task<List<Character>> GetCharactersAsync(Expression<Func<Character, bool>> filter)
    {
        return _context.Collection.Find(filter).ToListAsync();
    }

    public async Task<Character?> GetCharacterAsync(string id)
    {
        Character? character = await _context.Collection.Find(character => character.Id == id).FirstAsync();

        return character ?? null;
    }

    public Task<Character> CreateCharacterAsync(Character character)
    {
        return _context.Collection.InsertOneAsync(character).ContinueWith(_ => character);
    }

    public Task UpdateCharacterAsync(string id, Character characterIn)
    {
        return _context.Collection.ReplaceOneAsync(character => character.Id == id, characterIn);
    }

    public Task DeleteCharacterAsync(string id)
    {
        return _context.Collection.DeleteOneAsync(character => character.Id == id);
    }
    
    public Task DeleteCharactersAsync(Expression<Func<Character, bool>> filter)
    {
        return _context.Collection.DeleteManyAsync(filter);
    }
}