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
    public decimal SalaryPerHour { get; set; }
    [Required]
    public int HoursPerDay { get; set; }
    [Required]
    public int DaysPerWeek { get; set; }
    [Required]
    public int PayrollId { get; set; }
    [Required]
    public bool StatusId { get; set; }
    public string Status { get; set; }
}