using Microsoft.EntityFrameworkCore;
using RinkuHRApp.Data;
using RinkuHRApp.Models;

namespace RinkuHRApp.Services;

public class EmployeeService : IEmployeeService
{
    private readonly HumanResourcesContext _hrContext;

    public EmployeeService(HumanResourcesContext hrContext)
    {
        _hrContext = hrContext;
    }

    private void Save(EmployeeViewModel model, bool isNew = true) {
        try {
            Employee employee = new Employee {
                    Id = model.Id,
                    FullName = model.FullName,
                    SalaryPerHour = model.SalaryPerHour,
                    HoursPerDay = model.HoursPerDay,
                    DaysPerWeek = model.DaysPerWeek,
                    PayrollId = model.PayrollId,
                    StatusId = model.StatusId
                };
            _hrContext.Entry(employee).State = isNew ? EntityState.Added : EntityState.Modified;
            _hrContext.SaveChanges();
        }
        catch {
            throw;
        }
    }
    public void AddNew(EmployeeViewModel model)
    {
        Save(model);
    }

    public void Edit(EmployeeViewModel model)
    {
         Save(model, false);
    }

    public IEnumerable<EmployeeViewModel> GetAll(int payrollId)
    {
        return _hrContext.Employees.Include(x=> x.Position)
                                    .Where( x=> x.PayrollId == payrollId && x.StatusId == 1)
                                    .Select(x=> new EmployeeViewModel {
                                        Id = x.Id,
                                        FullName = x.FullName,
                                        Position = x.Position.Name,
                                        SalaryPerHour = x.SalaryPerHour,
                                        HoursPerDay = x.HoursPerDay,
                                        DaysPerWeek = x.DaysPerWeek
                                    })
                                    .ToList();
    }

    public EmployeeViewModel GetOne(int employeeId)
    {
        return _hrContext.Employees.Where( x=> x.Id == employeeId)
                                    .Select(x=> new EmployeeViewModel {
                                        Id = x.Id,
                                        FullName = x.FullName,
                                        PositionId = x.PositionId,
                                        SalaryPerHour = x.SalaryPerHour,
                                        HoursPerDay = x.HoursPerDay,
                                        DaysPerWeek = x.DaysPerWeek
                                    })
                                    .First();
    }
}
