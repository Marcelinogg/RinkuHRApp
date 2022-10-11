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
    [Range(1, int.MaxValue, ErrorMessage = "Please select a employee")]
    public int EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public int Sequence { get; set; }
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public int Times { get; set; }
    [Required]
    [Range(typeof(decimal), "0.01", "10000.00", ErrorMessage = "Please enter a value bigger than {1} and less than {2}")]
    public decimal Amount { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public int StatusId { get; set; }
}
