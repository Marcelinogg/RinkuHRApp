using RinkuHRApp.Data;
using RinkuHRApp.Models;

namespace RinkuHRApp.Services;

public class PositionService : IPositionService
{
    private readonly HumanResourcesContext _hrContext;
    
    public PositionService(HumanResourcesContext hrContext)
    {
        _hrContext = hrContext;
    }

    public IEnumerable<PositionViewModel> GetAll()
    {
        return _hrContext.Positions.Select(x=> new PositionViewModel {
                                        Id = x.Id,
                                        Name = x.Name
                                    })
                                    .ToList();
    }
}
