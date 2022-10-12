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

    // Comverts Object to string
    public string ToJSONString<T>(T model)
    {
        return JsonSerializer.Serialize<T>(model);
    }

     // Converts Json string into its representative object
    public T FromJSONStringToObject<T>(string model)
    {
        // To avoid error when filter is used on whole controller
        if(model != null) {
            return JsonSerializer.Deserialize<T>(model);
        }

        return default(T);
    }
}
