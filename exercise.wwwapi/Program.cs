using exercise.wwwapi.Data;
using exercise.wwwapi.Endpoints;
using exercise.wwwapi.Repository;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(); // Adds OpenAPI/Swagger support so we get interactive API documentation.
builder.Services.AddScoped<IProductRepository, ProductRepository>(); // Registers the Repository class as the implementation for the IRepository interface.
builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("bandsdb")); // Sets up the database

var app = builder.Build(); // Builds the web application using the settings and services you just configured.

// In development, enables OpenAPI endpoints and Swagger UI for interactive API docs        
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Demo API");
    });
    app.MapScalarApiReference();
}

app.UseHttpsRedirection(); // Redirects all HTTP requests to HTTPS for security.

app.ConfigureProduct(); // Activates the product-related API endpoints by calling extension methods that set up the routes.

app.Run(); // Starts the web application and begins listening for requests.