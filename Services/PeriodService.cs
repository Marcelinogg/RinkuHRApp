using RinkuHRApp.Data;
using RinkuHRApp.Models;

namespace RinkuHRApp.Services;

public class PeriodService : IPeriodService
{

    private readonly HumanResourcesContext _hrContext;

    public PeriodService(HumanResourcesContext hrContext)
    {
        _hrContext = hrContext;
    }

    private IEnumerable<PeriodViewModel> GetData(int payrollId, bool onlyActive = false)
    {
        return _hrContext.Periods.Where( x=> x.PayrollId == payrollId 
                                             && ((onlyActive && x.Active) || !onlyActive))
                                    .Select(x=> new PeriodViewModel {
                                        PayrollId = x.PayrollId,
                                        Id = x.Id,
                                        Name = x.Name,
                                        Active = x.Active
                                    })
                                    .ToList();
    }
    public IEnumerable<PeriodViewModel> GetAllActives(int payrollId)
    {
        return GetData(payrollId, true);
    }
}
