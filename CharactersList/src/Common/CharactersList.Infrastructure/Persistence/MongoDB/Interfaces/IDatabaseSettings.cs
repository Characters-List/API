namespace CharactersList.Infrastructure.Persistence.MongoDB.Interfaces;

public interface IDatabaseSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }
}