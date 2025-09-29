namespace Asmails.Services;

public interface ISaver
{
    void Save(string data);
}

public class Saver : ISaver
{
    public Saver(string args)
    {
        // Initialization logic if needed
        Console.WriteLine(args);
    }

    public void Save(string data)
    {
        // Implement saving logic here
        Console.WriteLine($"Data saved: {data}");
    }
}