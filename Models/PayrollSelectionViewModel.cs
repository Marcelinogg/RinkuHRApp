using System.ComponentModel.DataAnnotations;

namespace RinkuHRApp.Models;

public class PayrollSelectionViewModel
{
    [Required]
    public int PayrollId { get; set; }
    [Required]
    public string Payroll { get; set; }
    [Required]
    public int PeriodId { get; set; }
}