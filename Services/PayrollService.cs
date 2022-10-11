using RinkuHRApp.Data;
using RinkuHRApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

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

    public IEnumerable<PayrollConceptViewModel> GetPayrollConcepts(int payrollId, int periodId)
    {
        return _hrContext.VwPayrollConcepts.Where(x=> x.PayrollId == payrollId && x.PeriodId == periodId)
                                           .Select(x=> new PayrollConceptViewModel(){
                                            EmployeeId = x.EmployeeId,
                                            EmployeeFullName = x.EmployeeFullName,
                                            PositionName = x.PositionName,
                                            ConceptId = x.ConceptId,
                                            ConceptName = x.ConceptName,
                                            Amount = x.Amount,
                                            TypeConcept = x.TypeConcept
                                           })
                                           .ToList();
    }

    public int RunPayroll(int payrollId, int periodId, string employeeId)
    {
        string query = "Exec Payroll.CalculateSalary @PayrollId, @PeriodId,	@EmployeeIds";
        List<SqlParameter> parameters = new List<SqlParameter>() 
        {
            new SqlParameter() { ParameterName = "@PayrollId", Value = payrollId },
            new SqlParameter() { ParameterName = "@PeriodId", Value = periodId },
            new SqlParameter() { ParameterName = "@EmployeeIds", Value = employeeId == null ? "" : employeeId},
        };
        
        return _hrContext.Database.ExecuteSqlRaw(query, parameters.ToArray());
    }
}
