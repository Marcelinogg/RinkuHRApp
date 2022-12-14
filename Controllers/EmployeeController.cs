using Microsoft.AspNetCore.Mvc;
using RinkuHRApp.Filters;
using RinkuHRApp.Models;
using RinkuHRApp.Services;

namespace RinkuHRApp.Controllers;

[CheckSelectedPayrollAtribute]
public class EmployeeController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ISession _session => _httpContextAccessor.HttpContext.Session;
    private readonly ILogger<EmployeeController> _logger;
    
    private readonly IEmployeeService _employeeService;
    private readonly IPositionService _positionService;
    private readonly PayrollSelectionViewModel _selectedPayroll;

    public EmployeeController(
        IHttpContextAccessor httpContextAccessor,
        ILogger<EmployeeController> logger,
        IEmployeeService employeeService,
        IPositionService positionService,
        IPayrollService payrollService
        )
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _employeeService = employeeService;
        _positionService = positionService;
        _selectedPayroll =  _employeeService.FromJSONStringToObject<PayrollSelectionViewModel>(
            _session.GetString("SelectedPayroll")
        );
    }

    [HttpGet]
    public IActionResult Index()
    {
        GetCatalogsToView();
        return View(new EmployeeViewModel(){                // Creates a default object to the view
            SalaryPerHour = 30,
            HoursPerDay = 8,
            DaysPerWeek = 6,
            PayrollId = _selectedPayroll.PayrollId,
            StatusId = true
        });
    }

    // Search a employee by ID
    [HttpGet]
    public IActionResult SearchEmployee(int id)
    {
        EmployeeViewModel employee = _employeeService.GetOne(id);
        GetCatalogsToView("EditEmployee");

        return View("Index", employee);
    }

    [HttpPost]
    public IActionResult NewEmployee(EmployeeViewModel model)
    {
        if(ModelState.IsValid) {
            _employeeService.AddNew(model);
            TempData["Done"] = "Empleado agregado exitosamente";

            return RedirectToAction("Index");
        }

        GetCatalogsToView();
        return View("Index", model);
    }

    [HttpPost]
    public IActionResult EditEmployee(EmployeeViewModel model)
    {
        if(ModelState.IsValid) {
            _employeeService.Edit(model);
            TempData["Done"] = "Empleado actualizado exitosamente";
            
            return RedirectToAction("Index");
        }
        
        GetCatalogsToView("EditEmployee");
        return View("Index", model);
    }
    
    // This method is used to avoid repetitive data
    private void GetCatalogsToView(string action = "NewEmployee")
    {
        ViewBag.Positions = _positionService.GetAll();                                  // Retrieve data from the positon catalog
        ViewBag.Employees = _employeeService.GetAll(_selectedPayroll.PayrollId);        // Retrieve data from the employee catalog by payroll
        ViewBag.Action = action;                                                        // It is the form action
        TempData["PayrollLabel"] = _selectedPayroll.Payroll;
    }
}