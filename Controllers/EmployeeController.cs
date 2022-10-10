using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RinkuHRApp.Models;
using RinkuHRApp.Services;

namespace RinkuHRApp.Controllers;

public class EmployeeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IEmployeeService _employeeService;
    private readonly IPositionService _positionService;
    private const int PAYROLL = 1;

    public EmployeeController(
        ILogger<HomeController> logger,
        IEmployeeService employeeService,
        IPositionService positionService
        )
    {
        _logger = logger;
        _employeeService = employeeService;
        _positionService = positionService;
    }

    [HttpGet]
    public IActionResult Employee()
    {
        ViewBag.Positions = _positionService.GetAll();
        ViewBag.Employees = _employeeService.GetAll(PAYROLL);

        return View(new EmployeeViewModel(){
            SalaryPerHour = 30,
            HoursPerDay = 8,
            DaysPerWeek = 6,
            PayrollId = PAYROLL,
            StatusId = 1
        });
    }

    [HttpGet]
    public IActionResult SearchEmployee(int id)
    {
        EmployeeViewModel employee = _employeeService.GetOne(id);
        ViewBag.Positions = _positionService.GetAll();
        ViewBag.Employees = _employeeService.GetAll(PAYROLL);

        return View("Employee", employee);
    }

    [HttpPost]
    public IActionResult EmployeeNew(EmployeeViewModel model)
    {
        if(ModelState.IsValid) {
            _employeeService.AddNew(model);
            TempData["Done"] = "Empleado agrega exitosamente";

            return RedirectToAction("Employee");
        }

        return View("Employee", model);
    }

    [HttpPost]
    public IActionResult EmployeeEdit(EmployeeViewModel model)
    {
        if(ModelState.IsValid) {
            _employeeService.Edit(model);
            TempData["Done"] = "Empleado actualizado exitosamente";
            
            return RedirectToAction("Employee");
        }

        return View("Employee", model);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}