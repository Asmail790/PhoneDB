using Asmails.Services;
using AsmailServices;
using DataBase;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhoneDB.Pages;

public class Experiment : PageModel
{
    private ILoggerService _commentService;
    private PhoneDbContext _context;

    private IDBFactory _dBFactory;
    private Func<string, IPaymentService> _paymentService;
    private ISaver _saver;

    public Experiment(ISaver saver, ILoggerService commentService, IDBFactory dbFactory,
        Func<string, IPaymentService> payment, PhoneDbContext context)

    {
        _commentService = commentService;
        _saver = saver;
        _dBFactory = dbFactory;
        _paymentService = payment;
        _context = context;
    }

    public void OnGet()
    {
        _dBFactory.GetDBInstance().Connect();
        _commentService.Send("John Doe");
        _saver.Save("Sample data to save");
        _paymentService("PayPal").pay();
    }
}