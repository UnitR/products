using Microsoft.EntityFrameworkCore;
using TechTest.Data.Models;

namespace TechTest.Data.Repositories;

public class ProductRepository(TechTestDbContext dbContext) : IProductRepository
{
    public async Task<ICollection<Product>> GetAll()
    {
        return await dbContext
            .Products
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<bool> AddAsync(Product product)
    {
        product.DateAdded = DateTime.UtcNow.Date;
        dbContext.Products.Add(product);
        var inserted = await dbContext.SaveChangesAsync();

        return inserted > 0;
    }

    public async Task<ICollection<CategoryStock>> GetByCategoryAsync()
    {
        return await dbContext
            .Categories
            .GroupBy(c => c.Name)
            .Select(
                g => new CategoryStock
                {
                    CategoryName = g.Key,
                    TotalStock = g.Select(c => c.Products.Sum(p => p.Quantity)).First(),
                })
            .ToListAsync();
    }

    public async Task<ICollection<DateProductAdded>> GetByDateAddedAsync()
    {
        var startOfYear = DateTime.UtcNow.Subtract(TimeSpan.FromDays(365));

        return await dbContext
            .Products
            .Where(p => p.DateAdded >= startOfYear)
            .GroupBy(p => p.DateAdded.Date)
            .Select(
                g => new DateProductAdded
                {
                    Timeframe = GetTimeFrame(g.Key),
                    ProductCount = g.Count(),
                })
            .ToListAsync();
    }

    private static string GetTimeFrame(DateTime dateAdded)
    {
        if (dateAdded >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(7)))
        {
            return "week";
        }
        else if (dateAdded >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(180)))
        {
            return "halfyear";
        }
        else if (dateAdded >= DateTime.UtcNow.Subtract(TimeSpan.FromDays(365))) //TODO broken for leap years
        {
            return "year";
        }

        throw new ArgumentException();
    }
}
