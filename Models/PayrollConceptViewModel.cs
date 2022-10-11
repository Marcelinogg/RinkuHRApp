namespace RinkuHRApp.Models;

public class PayrollConceptViewModel
{
    public int? EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public string PositionName { get; set; }
    public int PayrollId { get; set; }
    public string Payroll { get; set; }
    public int PeriodId { get; set; }
    public int ConceptId { get; set; }
    public string ConceptName { get; set; }
    public decimal Amount { get; set; }
    public string TypeConcept { get; set; }
}