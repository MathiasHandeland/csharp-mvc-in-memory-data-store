using exercise.wwwapi.Data;
using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.Repository
{
    public class ProductRepository : IProductRepository
    {
        private DataContext _db;
        public ProductRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<List<Product>> GetAsync(string? category = null)
        {
            if (string.IsNullOrWhiteSpace(category))
                return await _db.Products.ToListAsync(); // if no category is provided we return all products from the database

            return await _db.Products // if a category is provided we only get products within that category
                .Where(p => p.Category != null && p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)) // OrdinalIgnoreCase is used to ignore case sensitivity
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _db.Products.FindAsync(id); // Retrieves a product by its ID from the database
        }

        public async Task<Product> DeleteAsync(int id)
        {
            var targetProduct = await _db.Products.FindAsync(id);
            if (targetProduct == null) return null; // Returns null if the product is not found
            _db.Products.Remove(targetProduct);
            await _db.SaveChangesAsync(); // Saves changes to the database asynchronously
            return targetProduct;
        }

        public async Task<Product> AddAsync(Product model)
        {
            await _db.Products.AddAsync(model); // Adds a new product to the database
            await _db.SaveChangesAsync(); // Saves changes to the database
            return model; // Returns the added product
        }

        public async Task<Product> UpdateAsync(int id, Product model)
        {
            var targetProduct = await _db.Products.FindAsync(id);
            targetProduct.Name = model.Name; // Updates the product's name
            targetProduct.Category = model.Category; // Updates the product's category
            targetProduct.Price = model.Price; // Updates the product's price

            await _db.SaveChangesAsync(); // Saves changes to the database asynchronously
            return targetProduct; // Returns the updated product
        }
    }
}