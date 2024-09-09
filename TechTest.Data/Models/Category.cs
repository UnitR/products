using System.ComponentModel.DataAnnotations;

namespace TechTest.Data.Models;

public sealed class Category
{
    public int CategoryId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
