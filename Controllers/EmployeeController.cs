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
    private readonly IPayrollService _payrollService;

    public EmployeeController(
        ILogger<HomeController> logger,
        IEmployeeService employeeService,
        IPositionService positionService,
        IPayrollService payrollService
        )
    {
        _logger = logger;
        _employeeService = employeeService;
        _positionService = positionService;
        _payrollService = payrollService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        GetCatalogsToView();
        ViewBag.Action = "EmployeeNew";

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
        GetCatalogsToView();
        EmployeeViewModel employee = _employeeService.GetOne(id);
        ViewBag.Action = "EmployeeEdit";

        return View("Index", employee);
    }

    [HttpPost]
    public IActionResult EmployeeNew(EmployeeViewModel model)
    {
        if(ModelState.IsValid) {
            _employeeService.AddNew(model);
            TempData["Done"] = "Empleado agrega exitosamente";

            return RedirectToAction("Index");
        }

        GetCatalogsToView();
        return View("Index", model);
    }

    [HttpPost]
    public IActionResult EmployeeEdit(EmployeeViewModel model)
    {
        if(ModelState.IsValid) {
            _employeeService.Edit(model);
            TempData["Done"] = "Empleado actualizado exitosamente";
            
            return RedirectToAction("Index");
        }

        return View("Index", model);
    }

    private void GetCatalogsToView() {
        IEnumerable<PayrollViewModel> payrolls = _payrollService.GetAll();
        ViewBag.Payrolls = payrolls;
        ViewBag.Positions = _positionService.GetAll();
        ViewBag.Employees = _employeeService.GetAll(payrolls.First().Id);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}