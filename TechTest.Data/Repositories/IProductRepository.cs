using TechTest.Data.Models;

namespace TechTest.Data.Repositories;

public interface IProductRepository
{
    Task<ICollection<Product>> GetAll();
    Task<bool> AddAsync(Product product);
    Task<ICollection<CategoryStock>> GetByCategoryAsync();
    Task<ICollection<DateProductAdded>> GetByDateAddedAsync();
}
