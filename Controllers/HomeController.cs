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


    [HttpGet]
    public IActionResult PayrollSelect()
    {
       GetCatalogsToView();
        return View(new PayrollSelectionViewModel());
    }

    [HttpPost]
    public IActionResult PayrollSelect(PayrollSelectionViewModel model)
    {
        if(ModelState.IsValid) {
            HttpContext.Session.SetString("PayrollSelected", _periodService.ToJSONString<PayrollSelectionViewModel>(model));
            string x = HttpContext.Session.GetString("PayrollSelected");
            return RedirectToAction("Index", "Employee");
        }

        GetCatalogsToView();
        return View(model);
    }

    private void GetCatalogsToView() 
    {
        ViewBag.Payrolls = _payrollService.GetAll();
        ViewBag.PeriodsStr = _periodService.ToJSONString<IEnumerable<PeriodViewModel>>(
            _periodService.GetAllActives()
        );
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
