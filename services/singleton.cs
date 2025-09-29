namespace AsmailServices;

public interface IDB
{
    void Connect();
}

public interface IDBFactory
{
    IDB GetDBInstance();
}

internal class DB : IDB
{
    private readonly string _key;

    public DB(string key)
    {
        _key = key;
    }

    public void Connect()
    {
        // Implement connection logic here
        Console.WriteLine($"Connected to the database using key: {_key}");
    }
}

internal class DBFactory : IDBFactory
{
    private readonly string _key;

    public DBFactory(string key)
    {
        _key = key;
        Console.WriteLine($"DBFactory created with key: {_key}");
    }

    public IDB GetDBInstance()
    {
        return new DB(_key);
    }
}