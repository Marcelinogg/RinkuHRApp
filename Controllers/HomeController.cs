using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RinkuHRApp.Models;
using RinkuHRApp.Services;

namespace RinkuHRApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPayrollService _payrollService;
    private readonly IPeriodService _periodService;
    private readonly IEmployeeService _employeeService;


    public HomeController(
        ILogger<HomeController> logger,
        IPayrollService payrollService,
        IPeriodService periodService
    )
    {
        _logger = logger;
        _payrollService = payrollService;
        _periodService = periodService;
    }

     // The initial  method, when the payroll is selected then is redirected to principal view
    [HttpGet]
    public IActionResult PayrollSelect()
    {
       GetCatalogsToView();
        return View(new PayrollSelectionViewModel());
    }

    [HttpPost]
    public IActionResult PayrollSelect(PayrollSelectionViewModel model)
    {
        if(ModelState.IsValid) {                                    // Converts object to string to be saved
            HttpContext.Session.SetString("SelectedPayroll", _periodService.ToJSONString<PayrollSelectionViewModel>(model));
            return RedirectToAction("Index", "Employee");
        }

        GetCatalogsToView();
        return View(model);
    }

    private void GetCatalogsToView() 
    {
        ViewBag.Payrolls = _payrollService.GetAll();                // Retrieve data from the payroll catalog
        ViewBag.PeriodsStr = _periodService.ToJSONString<IEnumerable<PeriodViewModel>>(     // The retrieve data is converted in string to be used to the view
            _periodService.GetAllActives()
        );
        PayrollSelectionViewModel selectedPayroll = _periodService.FromJSONStringToObject<PayrollSelectionViewModel>(HttpContext.Session.GetString("SelectedPayroll"));
        
        if(selectedPayroll != null) {
            TempData["PayrollLabel"] = selectedPayroll.Payroll;
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
