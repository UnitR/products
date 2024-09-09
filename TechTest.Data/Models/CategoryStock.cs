using System.ComponentModel.DataAnnotations.Schema;

namespace TechTest.Data.Models;

[NotMapped]
public record CategoryStock
{
    public string CategoryName { get; init; } = string.Empty;
    public int TotalStock { get; init; }
}
