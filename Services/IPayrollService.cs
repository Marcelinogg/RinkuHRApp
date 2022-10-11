using RinkuHRApp.Models;

namespace RinkuHRApp.Services;

public interface IPayrollService
{
    IEnumerable<PayrollViewModel> GetAll();
    IEnumerable<PayrollConceptViewModel> GetPayrollConcepts(PayrollCalculationViewModel model);
    int RunPayroll(PayrollCalculationViewModel model);
}
