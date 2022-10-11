using Microsoft.EntityFrameworkCore;
using RinkuHRApp.Data;
using RinkuHRApp.Models;

namespace RinkuHRApp.Services;

public class TransactionService : ITransactionService
{
    private readonly HumanResourcesContext _hrContext;

    public TransactionService(HumanResourcesContext hrContext)
    {
        _hrContext = hrContext;
    }

    private void Save(TransactionViewModel model, bool isNew = true) {
        try {
            Transaction transaction = new Transaction {
                PayrollId = model.PayrollId,
                PeriodId = model.PeriodId,
                ConceptId = model.ConceptId,
                EmployeeId = model.EmployeeId,
                Sequence = model.Sequence,
                Times = model.Times,
                Amount = model.Amount,
                UserId = model.UserId,
                CreatedDate = DateTime.Now,
                StatusId = model.StatusId
                };
            _hrContext.Entry(transaction).State = isNew ? EntityState.Added : EntityState.Modified;
            _hrContext.SaveChanges();
        }
        catch {
            throw;
        }
    }
    public void AddNew(TransactionViewModel model)
    {
        model.UserId = "App User";
        model.CreatedDate = DateTime.Now;
        model.StatusId = 1;
        model.Sequence = _hrContext.Transactions.Where(x=> x.PayrollId == model.PayrollId && x.PeriodId == model.PeriodId
                                                      && x.ConceptId == model.ConceptId && x.EmployeeId == model.EmployeeId)
                                                .Max(x=> (int?)x.Sequence).GetValueOrDefault() + 1;
        Save(model);
    }

    public void Edit(TransactionViewModel model)
    {
       if(model.Sequence == 0) {
        AddNew(model);

        return;
       }

        model.UserId = "App User";
        model.CreatedDate = DateTime.Now;
        model.StatusId = 1;

        Save(model, false);
    }

    public IEnumerable<TransactionViewModel> GetAllActives(int payrollId, int peridoId)
    {
        return _hrContext.Transactions.Include(x=> x.Concept)
                                    .Include(x=> x.Employee)
                                    .Include(x=> x.Employee.Position)
                                    .Where(x=> x.PayrollId == payrollId && x.PeriodId == peridoId && x.StatusId == 1)
                                    .Select(x=> new TransactionViewModel {
                                        PayrollId = x.PayrollId,
                                        PeriodId = x.PeriodId,
                                        EmployeeId = x.EmployeeId,
                                        EmployeeFullName = x.Employee.FullName,
                                        PositonName = x.Employee.Position.Name,
                                        Sequence = x.Sequence,
                                        ConceptId = x.ConceptId,
                                        Concept = x.Concept.Name,
                                        Times = x.Times,
                                        Amount = x.Amount
                                    })
                                    .ToList();
    }

    public TransactionViewModel GetOne(int payrollId, int peridoId, int conceptId, int employeeId, int sequence)
    {
        return _hrContext.Transactions.Include(x=> x.Employee)
                                    .Where( x=> x.PayrollId == payrollId && x.PeriodId == peridoId
                                                && x.ConceptId == conceptId && x.EmployeeId == employeeId && x.Sequence == sequence)
                                    .Select(x=> new TransactionViewModel {
                                        PayrollId = x.PeriodId,
                                        PeriodId = x.PeriodId,
                                        ConceptId = x.ConceptId,
                                        EmployeeId = x.EmployeeId,
                                        EmployeeFullName = x.Employee.FullName,
                                        Sequence = x.Sequence,
                                        Times = x.Times,
                                        Amount = x.Amount,
                                    })
                                    .First();
    }
}
