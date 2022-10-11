using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RinkuHRApp.Models;
using RinkuHRApp.Services;

namespace RinkuHRApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly IPayrollService _payrollService;
        private readonly IPeriodService _periodService;
        private readonly IEmployeeService _employeeService;
        private readonly ITransactionService _transactionService;
        private readonly int PAYROLL;

        public TransactionController(
            ILogger<TransactionController> logger,
            IPayrollService payrollService,
            IPeriodService periodService,
            IEmployeeService employeeService,
            ITransactionService transactionService
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
            GetCatalogsToView();
            return View(new TransactionViewModel(){
                ConceptId = 2,
                Amount = 5.00M
            });
        }

        [HttpGet]
        public IActionResult SearchTransaction(int payrollId, int periodId, int conceptId, int employeeId, int sequence)
        {
            TransactionViewModel transaction = _transactionService.GetOne(payrollId, periodId, conceptId, employeeId, sequence);
            GetCatalogsToView("EditTransaction");
            
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

            GetCatalogsToView();
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

            GetCatalogsToView("EditTransaction");
            return View("Index", model);
        }

        private void GetCatalogsToView(string action =  "NewTransaction")
        {
            ViewBag.Payrolls = _payrollService.GetAll();
            IEnumerable<PeriodViewModel> periods = _periodService.GetAllActives(PAYROLL);
            ViewBag.Periods = periods;
            IEnumerable<EmployeeViewModel> employees = _employeeService.GetAllActives(PAYROLL);
            ViewBag.Employees = employees;
            ViewBag.EmployeesStr = _employeeService.SerializeAllActives(employees);
            ViewBag.Transactions = _transactionService.GetAllActives(PAYROLL, periods.First().Id);
            ViewBag.Action = action;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}