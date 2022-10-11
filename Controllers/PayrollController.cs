using Microsoft.AspNetCore.Mvc;
using RinkuHRApp.Models;
using RinkuHRApp.Services;

namespace RinkuHRApp.Controllers
{
    public class PayrollController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly ILogger<PayrollController> _logger;

        private readonly IPayrollService _payrollService;
        private readonly IEmployeeService _employeeService;
        private readonly PayrollSelectionViewModel _payrollSelected;

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
            _payrollSelected =  _employeeService.FromJSONStringToObject<PayrollSelectionViewModel>(
                _session.GetString("PayrollSelected")
            );
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Employees = _employeeService.GetAllActives(_payrollSelected.PayrollId);
            ViewBag.PayrollConcepts = _payrollService.GetPayrollConcepts(
                _payrollSelected.PayrollId,
                _payrollSelected.PeriodId
            );
            TempData["PayrollLabel"] = _payrollSelected.Payroll;

            return View();
        }

        [HttpPost]
        public IActionResult Index(string employeeId)
        {
            TempData["rowsAffected"] = _payrollService.RunPayroll(_payrollSelected.PayrollId, _payrollSelected.PeriodId, employeeId);
                
            return RedirectToAction("Index");
        }
    }
}