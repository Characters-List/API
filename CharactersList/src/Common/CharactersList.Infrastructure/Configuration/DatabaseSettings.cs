namespace CharactersList.Infrastructure.Configuration;

public class DatabaseSettings
{
    public string Host { get; set; }
    public string Port { get; set; }
    public string AuthDatabase { get; set; }
    public string DatabaseName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
    public string ConnectionString => $"mongodb://{Username}:{Password}@{Host}:{Port}/{DatabaseName}?authSource={AuthDatabase}";
}