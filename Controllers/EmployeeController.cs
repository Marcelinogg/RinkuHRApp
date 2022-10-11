using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RinkuHRApp.Models;
using RinkuHRApp.Services;

namespace RinkuHRApp.Controllers;

public class EmployeeController : Controller
{
    private readonly ILogger<EmployeeController> _logger;
    private readonly IEmployeeService _employeeService;
    private readonly IPositionService _positionService;
    private readonly IPayrollService _payrollService;
    private readonly int PAYROLL;

    public EmployeeController(
        ILogger<EmployeeController> logger,
        IEmployeeService employeeService,
        IPositionService positionService,
        IPayrollService payrollService
        )
    {
        _logger = logger;
        _employeeService = employeeService;
        _positionService = positionService;
        _payrollService = payrollService;
        PAYROLL =  _payrollService.GetAll().First().Id;
    }

    [HttpGet]
    public IActionResult Index()
    {
        GetCatalogsToView();
        return View(new EmployeeViewModel(){
            SalaryPerHour = 30,
            HoursPerDay = 8,
            DaysPerWeek = 6,
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
    
    private void GetCatalogsToView(string action = "NewEmployee") {
        ViewBag.Payrolls = _payrollService.GetAll();
        ViewBag.Positions = _positionService.GetAll();
        ViewBag.Employees = _employeeService.GetAll(PAYROLL);
        ViewBag.Action = action;
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}