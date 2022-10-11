using Microsoft.AspNetCore.Mvc;
using RinkuHRApp.Models;
using RinkuHRApp.Services;

namespace RinkuHRApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly ILogger<TransactionController> _logger;
        
        private readonly IEmployeeService _employeeService;
        private readonly ITransactionService _transactionService;
        private readonly PayrollSelectionViewModel _payrollSelected;

        public TransactionController(
            IHttpContextAccessor httpContextAccessor,
            ILogger<TransactionController> logger,
            IEmployeeService employeeService,
            ITransactionService transactionService
        )
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _employeeService = employeeService;
            _transactionService = transactionService;
           _payrollSelected =  _employeeService.FromJSONStringToObject<PayrollSelectionViewModel>(
                _session.GetString("PayrollSelected")
            );
        }

        [HttpGet]
        public IActionResult Index()
        {
            GetCatalogsToView();
            return View(new TransactionViewModel(){
                PayrollId = _payrollSelected.PayrollId,
                PeriodId = _payrollSelected.PeriodId,
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
            IEnumerable<EmployeeViewModel> employees = _employeeService.GetAllActives(_payrollSelected.PayrollId);
            ViewBag.Employees = employees;
            ViewBag.EmployeesStr = _employeeService.ToJSONString<IEnumerable<EmployeeViewModel>>(employees);
            ViewBag.Transactions = _transactionService.GetAllActives(_payrollSelected.PayrollId, _payrollSelected.PeriodId);
            ViewBag.Action = action;
            TempData["PayrollLabel"] = _payrollSelected.Payroll;
        }
    }
}