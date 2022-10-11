using RinkuHRApp.Models;

namespace RinkuHRApp.Services;

public interface IPayrollService
{
    IEnumerable<PayrollViewModel> GetAll();
    IEnumerable<PayrollConceptViewModel> GetPayrollConcepts(int payrollId, int periodId);
    int RunPayroll(int payrollId, int periodId, string employeeId);
}
