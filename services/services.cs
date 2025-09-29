using Asmails.Services;

namespace AsmailServices;

public static class ServiceExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<ISaver, Saver>(s => new Saver(DateTime.Now.ToString()));
        services.AddSingleton<IDBFactory, DBFactory>(s => new DBFactory("my-secret-key"));
        services.AddSingleton<INameDisplayService, NameDisplayService>();
        services.AddTransient<PayPalPaymentService>();
        services.AddTransient<SwishPaymentService>();

        services.AddTransient<Func<string, IPaymentService>>(serviceProvider => key =>
        {
            switch (key)
            {
                case "PayPal":
                    return serviceProvider.GetRequiredService<PayPalPaymentService>();
                case "Swish":
                    return serviceProvider.GetRequiredService<SwishPaymentService>();
                default:
                    throw new ArgumentException($"Payment service '{key}' is not registered.");
            }
        });


        return services.AddTransient<ILoggerService, LoggerService>();
    }
}