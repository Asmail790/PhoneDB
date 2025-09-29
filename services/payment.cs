namespace Asmails.Services;

public interface IPaymentService
{
    public void pay();
}

public class PayPalPaymentService : IPaymentService
{
    public void pay()
    {
        Console.WriteLine("Payment made using PayPal.");
    }
}

public class SwishPaymentService : IPaymentService
{
    public void pay()
    {
        Console.WriteLine("Payment made using Swish.");
    }
}