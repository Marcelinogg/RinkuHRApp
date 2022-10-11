using System.ComponentModel.DataAnnotations;

namespace RinkuHRApp.Models;
public class TransactionViewModel
{
    [Required]
    public int PayrollId { get; set; }
    public string Payroll { get; set; }
    [Required]
    public int PeriodId { get; set; }
    [Required]
    public int ConceptId { get; set; }
    public string Concept { get; set; }
    [Required]
    public int EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    [Required]
    public int Times { get; set; }
    [Required]
    public decimal Amount { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public int StatusId { get; set; }
}
