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
        Save(model);
    }

    public void Edit(TransactionViewModel model)
    {
        Save(model, false);
    }

    public IEnumerable<TransactionViewModel> GetAll(int payrollId, int peridoId)
    {
        return _hrContext.Transactions.Include(x=> x.Concept)
                                    .Where(x=> x.PayrollId == payrollId && x.PeriodId == peridoId && x.StatusId == 1)
                                    .Select(x=> new TransactionViewModel {
                                        ConceptId = x.ConceptId,
                                        Concept = x.Concept.Name,
                                        Times = x.Times,
                                        Amount = x.Amount
                                    })
                                    .ToList();
    }

    public TransactionViewModel GetOne(int payrollId, int peridoId, int conceptId, int employeeId)
    {
        return _hrContext.Transactions.Where( x=> x.PayrollId == payrollId && x.PeriodId == peridoId && x.ConceptId == conceptId && x.EmployeeId == employeeId)
                                    .Select(x=> new TransactionViewModel {
                                        PayrollId = x.PeriodId,
                                        PeriodId = x.PeriodId,
                                        ConceptId = x.ConceptId,
                                        EmployeeId = x.EmployeeId,
                                        Times = x.Times,
                                        Amount = x.Amount,
                                    })
                                    .First();
    }
}
