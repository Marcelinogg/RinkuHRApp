using Microsoft.AspNetCore.Mvc;
using RinkuHRApp.Filters;
using RinkuHRApp.Models;
using RinkuHRApp.Services;

namespace RinkuHRApp.Controllers;

[CheckSelectedPayrollAtribute]
public class TransactionController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ISession _session => _httpContextAccessor.HttpContext.Session;
    private readonly ILogger<TransactionController> _logger;
        
    private readonly IEmployeeService _employeeService;
    private readonly ITransactionService _transactionService;
    private readonly PayrollSelectionViewModel _selectedPayroll;

    public TransactionController(
        IHttpContextAccessor httpContextAccessor,
        ILogger<TransactionController> logger,
        IEmployeeService employeeService,
        ITransactionService transactionService
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _employeeService = employeeService;
        _transactionService = transactionService;
       _selectedPayroll =  _employeeService.FromJSONStringToObject<PayrollSelectionViewModel>(
            _session.GetString("SelectedPayroll")
        );
    }

    [HttpGet]
    public IActionResult Index()
    {
        GetCatalogsToView();
        return View(new TransactionViewModel(){                 // Creates a default object to the view
            PayrollId = _selectedPayroll.PayrollId,
            PeriodId = _selectedPayroll.PeriodId,
            ConceptId = 2,
            Amount = 5.00M
        });
    }

    // Search a transaction capture by IDs
    [HttpGet]
    public IActionResult SearchTransaction(int payrollId, int periodId, int conceptId, int employeeId, int sequence)
    {
        TransactionViewModel transaction = _transactionService.GetOne(payrollId, periodId, conceptId, employeeId, sequence);
        GetCatalogsToView("EditTransaction");
            
        return View("Index", transaction);
    }

    [HttpPost]
    public IActionResult NewTransaction(TransactionViewModel model)
    {
        if(ModelState.IsValid) {
            _transactionService.AddNew(model);
            TempData["Done"] = "Bono agregado exitosamente";

            return RedirectToAction("Index");
        }

        GetCatalogsToView();
        return View("Index", model);
    }

    [HttpPost]
    public IActionResult EditTransaction(TransactionViewModel model)
    {
        if(ModelState.IsValid) {
            _transactionService.Edit(model);
            TempData["Done"] = "Bono actualizado exitosamente";
                
            return RedirectToAction("Index");
        }

        GetCatalogsToView("EditTransaction");
        return View("Index", model);
    }

    // This method is used to avoid repetitive data
    private void GetCatalogsToView(string action =  "NewTransaction")
    {
        IEnumerable<EmployeeViewModel> employees = _employeeService.GetAllActives(_selectedPayroll.PayrollId);              // Retrieve data from the employee catalog by payroll
        ViewBag.Employees = employees;
        ViewBag.EmployeesStr = _employeeService.ToJSONString<IEnumerable<EmployeeViewModel>>(employees);                    // The retrieve data is converted in string to be used to the view
        ViewBag.Transactions = _transactionService.GetAllActives(_selectedPayroll.PayrollId, _selectedPayroll.PeriodId);    // retrieves data from active transaction captures
        ViewBag.Action = action;                                                                                            // It is the form action
        TempData["PayrollLabel"] = _selectedPayroll.Payroll;
    }
    }