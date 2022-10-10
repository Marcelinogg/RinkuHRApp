using System.ComponentModel.DataAnnotations;

namespace RinkuHRApp.Models;
public class TransactionViewModel
{
    [Required]
    public int PayrollId { get; set; }
    [Required]
    public int PeriodId { get; set; }
    [Required]
    public int ConceptId { get; set; }
    public string Concept { get; set; }
    [Required]
    public int EmployeeId { get; set; }
    [Required]
    public int Times { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    public DateTime CreatedDate { get; set; }
    [Required]
    public int StatusId { get; set; }
}
