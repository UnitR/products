using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TechTest.Data.Models;

[PrimaryKey("ProductId")]
public sealed class Product
{
    public int ProductId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    [Required]
    [MaxLength(12)]
    public string Sku { get; set; } = string.Empty;

    [Required]
    public int Quantity { get; set; }

    [ForeignKey(nameof(this.Category))]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    [Required]
    public DateTime DateAdded { get; set; }
}
