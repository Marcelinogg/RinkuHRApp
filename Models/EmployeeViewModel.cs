using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace RinkuHRApp.Models;

public class EmployeeViewModel
{
    public int Id { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    public int PositionId { get; set; }
    public string Position { get; set; }
    [Required]
    [Range(typeof(decimal), "0.1", "10000.00", ErrorMessage = "Please enter a salary between {1} and {2}")]
    public decimal SalaryPerHour { get; set; }
    [Required]
    [Range(1, 16, ErrorMessage = "Please enter an hour between {1} and {2}")]
    public int HoursPerDay { get; set; }
    [Required]
    [Range(1, 7, ErrorMessage = "Please enter a value between {1} and {2}")]
    public int DaysPerWeek { get; set; }
    [Required]
    public int PayrollId { get; set; }
    public string Payroll { get; set; }
    [Required]
    public bool StatusId { get; set; }
    public string Status { get; set; }
}