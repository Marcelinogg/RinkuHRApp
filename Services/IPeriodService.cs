using RinkuHRApp.Models;

namespace RinkuHRApp.Services;

public interface IPeriodService
{
    IEnumerable<PeriodViewModel> GetAllActives(int payrollId);
}
