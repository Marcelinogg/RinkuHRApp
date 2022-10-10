using RinkuHRApp.Models;

namespace RinkuHRApp.Services;

public interface IEmployeeService
{
    EmployeeViewModel GetOne(int employeeId);
    IEnumerable<EmployeeViewModel> GetAll(int payrollId);
    void AddNew(EmployeeViewModel model);
    void Edit(EmployeeViewModel model);
}