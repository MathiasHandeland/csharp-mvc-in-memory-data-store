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
            app.MapGet("/", GetProducts); // endpoint for requesting all products in the database
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
        public static async Task<IResult> GetProducts(IProductRepository repository)
        {
            var product = await repository.GetAsync(); // get all products from the repository
            return TypedResults.Ok(product); // send the products back as a response to client

        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        public static async Task<IResult> AddProduct(IProductRepository repository, ProductPost model)
        {
            Product entity = new Product();
            entity.Name = model.Name;
            entity.Category = model.Category;
            entity.Price = model.Price;

            var result = await repository.AddAsync(entity);

            // send back the url of the item just crated
            return TypedResults.Created($"https://localhost:7188/products/{entity.Id}", new { ProductName = model., ProductCategory = model.Category, ProductPrice = model.Price });
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
        public static async Task<IResult> UpdateProduct(IProductRepository repository, ProductPost model)
        {
            throw new NotImplementedException();
        }
    }
}