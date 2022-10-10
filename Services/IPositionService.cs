using RinkuHRApp.Models;

namespace RinkuHRApp.Services;

public interface IPositionService
{
         IEnumerable<PositionViewModel> GetAll();
}
