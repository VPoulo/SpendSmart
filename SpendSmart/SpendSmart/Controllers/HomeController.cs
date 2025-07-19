using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpendSmart.Models;

namespace SpendSmart.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly SpendSmartDBContext _context;


    // Constructor that takes a 
    public HomeController(ILogger<HomeController> logger, SpendSmartDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Expense()
    {
        var allExpenses = _context.Expenses.ToList();

        var totalExpenses = allExpenses.Sum(x => x.Value);
        ViewBag.Expenses = totalExpenses;

        return View(allExpenses);
    }

    public IActionResult CreateEditExpense(int? id)
    {
        if (id != null)
        {
            //Editing  -> load an expense by id
            var expenseinDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
            return View(expenseinDb);
        }

        return View();
    }

    public IActionResult CreateEditExpenseForm(Expense model)
    {
        if (model.Id == 0)
        {
            // Create new entry
            _context.Expenses.Add(model);
        }
        else
        {
            // Update existing entry
            _context.Expenses.Update(model);
        }

        _context.SaveChanges();

        return RedirectToAction("Expense");
    }

    public IActionResult DeleteExpense(int id)
    {
        var expenseinDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
        _context.Expenses.Remove(expenseinDb);
        _context.SaveChanges();

        return RedirectToAction("Expense");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
