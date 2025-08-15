using exercise.wwwapi.Models;

namespace exercise.wwwapi.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAsync(string? category);
        Task<Product> GetByIdAsync(int id);
        Task<Product> DeleteAsync(int id);
        Task<Product> UpdateAsync(int id, Product model);
        Task<Product> AddAsync(Product model);
    }
}
