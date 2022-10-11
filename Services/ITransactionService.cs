using RinkuHRApp.Models;

namespace RinkuHRApp.Services;

public interface ITransactionService
{
    TransactionViewModel GetOne(int payrollId, int peridoId, int conceptId, int employeeId, int sequence);
    IEnumerable<TransactionViewModel> GetAllActives(int payrollId, int peridoId);
    void AddNew(TransactionViewModel model);
    void Edit(TransactionViewModel model);
}
