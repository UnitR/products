using System.ComponentModel.DataAnnotations.Schema;

namespace TechTest.Data.Models;

[NotMapped]
public class DateProductAdded
{
    public string Timeframe { get; set; } = string.Empty;
    public int ProductCount { get; set; }
}
