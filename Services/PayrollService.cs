using RinkuHRApp.Data;
using RinkuHRApp.Models;

namespace RinkuHRApp.Services;

public class PayrollService : IPayrollService
{
    private readonly HumanResourcesContext _hrContext;

    public PayrollService(HumanResourcesContext hrContext)
    {
        _hrContext = hrContext;
    }

    public IEnumerable<PayrollViewModel> GetAll()
    {
        return _hrContext.Payrolls.Select(x=> new PayrollViewModel {
                                        Id = x.Id,
                                        Name = x.Name
                                    })
                                    .ToList();
    }
}
