using exercise.wwwapi.DTOs;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoints
{
    public static class ProductEndpoints
    {
        public static void ConfigureProduct(this WebApplication app)
        {
            var group = app.MapGroup("products"); // creates a group for product endpoints

            app.MapGet("/{id}", GetProductById); // endpoint for requesting a product
            app.MapGet("/", GetProducts); // endpoint for requesting all products in the database, optionally all products from a category
            app.MapPost("/", AddProduct); // endpoint for adding a product to the database
            app.MapDelete("/{id}", DeleteProduct); // endpoint for deleting a spesific product
            app.MapPut("/{id}", UpdateProduct); // endpoint for updating a spesific product
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetProductById(IProductRepository repository, int id)
        {
            var product = await repository.GetByIdAsync(id);
            if (product == null) return TypedResults.NotFound($"Product with ID {id} not found.");
            return TypedResults.Ok(product); // send the product back as a response to client
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetProducts(IProductRepository repository, [FromQuery] string? category)
        {
            var products = await repository.GetAsync(category);
            if (!products.Any()) return TypedResults.NotFound($"No products of the provided category {category} was found.");
            return TypedResults.Ok(products); // sends back the products either from a category or all products if no category is provided and the products list is not empty
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> DeleteProduct(IProductRepository repository, int id)
        {
            var product = await repository.DeleteAsync(id);
            if (product == null) return TypedResults.NotFound($"Product with ID {id} not found.");
            return TypedResults.Ok(product);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> AddProduct(IProductRepository repository, ProductPost model)
        {
            // check if the string the client provided for name doesn`t already exists in the database
            var existingsProduct = await repository.GetAsync(null); // no category provided, so we check all products if the name already exists
            if (existingsProduct.Any(p => p.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return TypedResults.BadRequest($"Product with name {model.Name} already exists");
            }

            // check if the value the client provided for price is an integer
           

            Product entity = new Product(); // create a new product entity to add to the database
            entity.Category = model.Category; // many products can have the same category so no need to check if the category is already in the database
            entity.Name = model.Name; 
            entity.Price = model.Price; // set the price of the product


            await repository.AddAsync(entity); // add the new product to the repository

            // send back the url of the product just created
            return TypedResults.Created($"https://localhost:7188/products/{entity.Id}", new { ProductName = model.Name, ProductCategory = model.Category, ProductPrice = model.Price });
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        public static async Task<IResult> UpdateProduct(IProductRepository repository, ProductPut model, int id)
        {
            Product existingProduct = await repository.GetByIdAsync(id);
            // update the existing product with the new values from the model the client sent
            if (model.Name != null) { existingProduct.Name = model.Name; }
            if (model.Category != null) { existingProduct.Category = model.Category; }
            if (model.Price != null) { existingProduct.Price = model.Price.Value; }

            var updatedProduct = await repository.UpdateAsync(id, existingProduct);

            return TypedResults.Ok(updatedProduct); // send the updated product back as a response to client
        }
    }
}