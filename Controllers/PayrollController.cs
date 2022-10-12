using Microsoft.AspNetCore.Mvc;
using RinkuHRApp.Filters;
using RinkuHRApp.Models;
using RinkuHRApp.Services;

namespace RinkuHRApp.Controllers;

[CheckSelectedPayrollAtribute]
public class PayrollController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ISession _session => _httpContextAccessor.HttpContext.Session;
    private readonly ILogger<PayrollController> _logger;

    private readonly IPayrollService _payrollService;
    private readonly IEmployeeService _employeeService;
    private readonly PayrollSelectionViewModel _selectedPayroll;

    public PayrollController(
        IHttpContextAccessor httpContextAccessor,
        ILogger<PayrollController> logger,
        IPayrollService payrollService,
        IEmployeeService employeeService
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _payrollService = payrollService;
        _employeeService = employeeService;
        _selectedPayroll =  _employeeService.FromJSONStringToObject<PayrollSelectionViewModel>(
        _session.GetString("SelectedPayroll")
        );
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.Employees = _employeeService.GetAllActives(_selectedPayroll.PayrollId);     // Retrieve data from the active employee catalog by payroll
        ViewBag.PayrollConcepts = _payrollService.GetPayrollConcepts(                       // Retrieve data from the calculated payroll by period
            _selectedPayroll.PayrollId,
            _selectedPayroll.PeriodId
        );
        TempData["PayrollLabel"] = _selectedPayroll.Payroll;

        return View();
    }

    // This method calls the main process to calculate payroll
    [HttpPost]
    public IActionResult Index(string employeeId)
    {
        TempData["rowsAffected"] = _payrollService.RunPayroll(_selectedPayroll.PayrollId, _selectedPayroll.PeriodId, employeeId);
                
        return RedirectToAction("Index");
    }
}