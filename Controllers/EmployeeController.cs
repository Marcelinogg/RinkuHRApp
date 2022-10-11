using Microsoft.AspNetCore.Mvc;
using RinkuHRApp.Models;
using RinkuHRApp.Services;

namespace RinkuHRApp.Controllers;

public class EmployeeController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ISession _session => _httpContextAccessor.HttpContext.Session;
    private readonly ILogger<EmployeeController> _logger;
    
    private readonly IEmployeeService _employeeService;
    private readonly IPositionService _positionService;
    private readonly PayrollSelectionViewModel _payrollSelected;

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
        _payrollSelected =  _employeeService.FromJSONStringToObject<PayrollSelectionViewModel>(
            _session.GetString("PayrollSelected")
        );
    }

    [HttpGet]
    public IActionResult Index()
    {
        GetCatalogsToView();
        return View(new EmployeeViewModel(){
            SalaryPerHour = 30,
            HoursPerDay = 8,
            DaysPerWeek = 6,
            PayrollId = _payrollSelected.PayrollId,
            StatusId = true
        });
    }

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
    
    private void GetCatalogsToView(string action = "NewEmployee")
    {
        ViewBag.Positions = _positionService.GetAll();
        ViewBag.Employees = _employeeService.GetAll(_payrollSelected.PayrollId);
        ViewBag.Action = action;
        TempData["PayrollLabel"] = _payrollSelected.Payroll;
    }
}