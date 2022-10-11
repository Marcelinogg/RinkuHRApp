using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RinkuHRApp.Models;
using RinkuHRApp.Services;

namespace RinkuHRApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPayrollService _payrollService;
        private readonly IPeriodService _periodService;
        private readonly IEmployeeService _employeeService;
        private readonly ITransactionService _transactionService;
        private readonly int PAYROLL;

        public TransactionController(
            ILogger<HomeController> logger,
            IPayrollService payrollService,
            IPeriodService periodService,
            IEmployeeService employeeService,
            TransactionService transactionService
        )
        {
            _logger = logger;
            _payrollService = payrollService;
            _periodService = periodService;
            _employeeService = employeeService;
            _transactionService = transactionService;
            PAYROLL =  _payrollService.GetAll().First().Id;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // GetCatalogsToView();
            ViewBag.Periods = _periodService.GetAllActives(PAYROLL);
            IEnumerable<EmployeeViewModel> employees = _employeeService.GetAllActives(PAYROLL);
            ViewBag.Employees = employees;
            ViewBag.EmployeeStr = _employeeService.SerializeAllActives(employees);
            ViewBag.Action = "NewTransaction";

            return View(new TransactionViewModel(){
                ConceptId = 2
            });
        }

        [HttpGet]
        public IActionResult SearchTransaction(int payrollId, int peridoId, int conceptId, int employeeId)
        {
            ViewBag.Periods = _periodService.GetAllActives(PAYROLL);
            IEnumerable<EmployeeViewModel> employees = _employeeService.GetAllActives(PAYROLL);
            ViewBag.Employees = employees;
            ViewBag.EmployeeStr = _employeeService.SerializeAllActives(employees);
            TransactionViewModel transaction = _transactionService.GetOne(payrollId, peridoId, conceptId, employeeId);
            ViewBag.Action = "EditTransaction";
            // GetCatalogsToView();
            return View("Index", transaction);
        }

        [HttpPost]
        public IActionResult NewTransaction(TransactionViewModel model)
        {
            if(ModelState.IsValid) {
                _transactionService.AddNew(model);
                TempData["Done"] = "Bono agregado exitosamente";

                return RedirectToAction("Index");
            }

            ViewBag.Periods = _periodService.GetAllActives(PAYROLL);
            IEnumerable<EmployeeViewModel> employees = _employeeService.GetAllActives(PAYROLL);
            ViewBag.Employees = employees;
            ViewBag.EmployeeStr = _employeeService.SerializeAllActives(employees);
            // GetCatalogsToView();
            return View("Index", model);
        }

        [HttpPost]
        public IActionResult EditTransaction(TransactionViewModel model)
        {
            if(ModelState.IsValid) {
                _transactionService.Edit(model);
                TempData["Done"] = "Bono actualizado exitosamente";
                
                return RedirectToAction("Index");
            }

            ViewBag.Periods = _periodService.GetAllActives(PAYROLL);
            IEnumerable<EmployeeViewModel> employees = _employeeService.GetAllActives(PAYROLL);
            ViewBag.Employees = employees;
            ViewBag.EmployeeStr = _employeeService.SerializeAllActives(employees);
            // GetCatalogsToView();
            return View("Index", model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}