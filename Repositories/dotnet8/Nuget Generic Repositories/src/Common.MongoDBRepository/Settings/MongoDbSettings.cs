namespace Common.MongoDBRepository.Settings;

public class MongoDBSettings
{
    public required string Host { get; set; }
    public int Port { get; set; }
    public string ConnectionString => $"mongodb://{Host}:{Port}";
}