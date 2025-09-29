namespace Asmails.Services;

public interface ILoggerService
{
    void Send(string comments);
}

public class LoggerService : ILoggerService
{
    private readonly DateTime date = DateTime.Now;

    void ILoggerService.Send(string comments)
    {
        Console.WriteLine($"{date.ToString()}-comments: {comments}");
    }
}