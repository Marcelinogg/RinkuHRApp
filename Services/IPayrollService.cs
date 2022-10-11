using RinkuHRApp.Models;

namespace RinkuHRApp.Services;

public interface IPayrollService
{
    IEnumerable<PayrollViewModel> GetAll();
}
