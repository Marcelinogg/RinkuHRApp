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

    public IEnumerable<PayrollConceptViewModel> GetPayrollConcepts(PayrollCalculationViewModel model)
    {
        return _hrContext.VwPayrollConcepts.Where(x=> x.PayrollId == model.PayrollId && x.PeriodId == model.PeriodId)
                                           .Select(x=> new PayrollConceptViewModel(){
                                            EmployeeId = x.EmployeeId,
                                            EmployeeFullName = x.EmployeeFullName,
                                            ConceptId = x.ConceptId,
                                            ConceptName = x.ConceptName,
                                            Amount = x.Amount,
                                            TypeConcept = x.TypeConcept
                                           })
                                           .ToList();
    }

    public int RunPayroll(PayrollCalculationViewModel model)
    {
        string query = "Exec Payroll.CalculateSalary @PayrollId, @PeriodId,	@EmployeeIds";
        List<SqlParameter> parameters = new List<SqlParameter>() 
        {
            new SqlParameter() { ParameterName = "@PayrollId", Value = model.PayrollId },
            new SqlParameter() { ParameterName = "@PeriodId", Value = model.PeriodId },
            new SqlParameter() { ParameterName = "@EmployeeIds", Value = model.EmployeeId == null ? "" : model.EmployeeId},
        };
        
        return _hrContext.Database.ExecuteSqlRaw(query, parameters.ToArray());
    }
}
