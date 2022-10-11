using RinkuHRApp.Models;

namespace RinkuHRApp.Services;

public interface IPeriodService
{
    string ToJSONString<T>(T model);
    // T ToObject<T>(T model);
    IEnumerable<PeriodViewModel> GetAllActives();
}
