using System.Text.Json;
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

    // Internal method to data access (take all periods o just active periods)
    private IEnumerable<PeriodViewModel> GetData(bool onlyActive = false)
    {
        return _hrContext.Periods.Where( x=> (onlyActive && x.Active) || !onlyActive)
                                    .Select(x=> new PeriodViewModel {
                                        PayrollId = x.PayrollId,
                                        Id = x.Id,
                                        Name = x.Name,
                                        Active = x.Active
                                    })
                                    .ToList();
    }
    
    public IEnumerable<PeriodViewModel> GetAllActives()
    {
        return GetData(true);
    }

    public string ToJSONString<T>(T model)
    {
        return JsonSerializer.Serialize<T>(model);
    }
}
