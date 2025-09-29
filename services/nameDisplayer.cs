namespace AsmailServices;

public interface INameDisplayService
{
    public string DisplayName(string firstName, string lastName);
}

public class NameDisplayService : INameDisplayService
{
    public string DisplayName(string firstName, string lastName)
    {
        return $"{firstName}-{lastName}";
    }
}