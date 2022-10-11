using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RinkuHRApp.Models;
using RinkuHRApp.Services;

namespace RinkuHRApp.Controllers
{
    public class PayrollController : Controller
    {
        private readonly ILogger<PayrollController> _logger;
        private readonly IPayrollService _payrollService;
        private readonly IPeriodService _periodService;
        private readonly IEmployeeService _employeeService;
        private readonly int PAYROLL;

        public PayrollController(
            ILogger<PayrollController> logger,
            IPayrollService payrollService,
            IPeriodService periodService,
            IEmployeeService employeeService
        )
        {
            _logger = logger;
            _payrollService = payrollService;
            _periodService = periodService;
            _employeeService = employeeService;
            PAYROLL =  _payrollService.GetAll().First().Id;
        }

        [HttpGet]
        public IActionResult Index()
        {
            GetCatalogsToView();
            return View(new PayrollCalculationViewModel());
        }

        [HttpPost]
        public IActionResult Index(PayrollCalculationViewModel model)
        {
            if(ModelState.IsValid) {
                _payrollService.RunPayroll(model);
                TempData["Done"] = "Nómina procesada exitosamente";
                
                return RedirectToAction("Index");
            }
            
            GetCatalogsToView();

            return View(new PayrollCalculationViewModel());
        }

        private void GetCatalogsToView()
        {
            ViewBag.Payrolls = _payrollService.GetAll();
            IEnumerable<PeriodViewModel> periods = _periodService.GetAllActives(PAYROLL);
            ViewBag.Periods = periods;
            ViewBag.Employees = _employeeService.GetAllActives(PAYROLL);
            ViewBag.PayrollConcepts = _payrollService.GetPayrollConcepts(new PayrollCalculationViewModel(){
                PayrollId = PAYROLL,
                PeriodId = periods.First().Id
            }
            );
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}