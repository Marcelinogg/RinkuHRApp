using System.Text.Json;
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

    // Internal method to data access (take all employess o just active employees)
    private IEnumerable<EmployeeViewModel> GetData(int payrollId, bool onlyActive = false)
    {
        return _hrContext.Employees.Include(x=> x.Position)
                                    .Include(x=> x.Payroll)
                                    .Where( x=> x.PayrollId == payrollId 
                                                && ((onlyActive && x.StatusId == 1) || !onlyActive))
                                    .Select(x=> new EmployeeViewModel {
                                        Id = x.Id,
                                        FullName = x.FullName,
                                        Position = x.Position.Name,
                                        SalaryPerHour = x.SalaryPerHour,
                                        HoursPerDay = x.HoursPerDay,
                                        DaysPerWeek = x.DaysPerWeek,
                                        PayrollId = x.PayrollId,
                                        Payroll = x.Payroll.Name,
                                        Status = x.Status.Name
                                    })
                                    .ToList();
    }

    private void Save(EmployeeViewModel model, bool isNew = true) {
        try {
            Employee employee = new Employee {
                    Id = model.Id,
                    FullName = model.FullName,
                    PositionId = model.PositionId,
                    SalaryPerHour = model.SalaryPerHour,
                    HoursPerDay = model.HoursPerDay,
                    DaysPerWeek = model.DaysPerWeek,
                    PayrollId = model.PayrollId,
                    StatusId = model.StatusId ? 1 : 2
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
        // Set the ID to the employee (the minimum ID si 1000001)
        int maxId = _hrContext.Employees.Max(x=> (int?)x.Id).GetValueOrDefault() + 1;
        model.Id = maxId == 1 ? 10000001 : maxId;

        Save(model);
    }

    public void Edit(EmployeeViewModel model)
    {
         Save(model, false);
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

    public IEnumerable<EmployeeViewModel> GetAllActives(int payrollId)
    {
        return GetData(payrollId, true);
    }

    public IEnumerable<EmployeeViewModel> GetAll(int payrollId)
    {
        return GetData(payrollId);
    }

    // Data access for an object by ID
    public EmployeeViewModel GetOne(int employeeId)
    {
        return _hrContext.Employees.Where( x=> x.Id == employeeId)
                                    .Select(x=> new EmployeeViewModel {
                                        Id = x.Id,
                                        FullName = x.FullName,
                                        PositionId = x.PositionId,
                                        SalaryPerHour = x.SalaryPerHour,
                                        HoursPerDay = x.HoursPerDay,
                                        DaysPerWeek = x.DaysPerWeek,
                                        PayrollId = x.PayrollId,
                                        StatusId = x.StatusId == 1
                                    })
                                    .First();
    }
}
