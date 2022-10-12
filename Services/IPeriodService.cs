using RinkuHRApp.Models;

namespace RinkuHRApp.Services;

public interface IPeriodService
{
    string ToJSONString<T>(T model);
    T FromJSONStringToObject<T>(string model);
    IEnumerable<PeriodViewModel> GetAllActives();
}
