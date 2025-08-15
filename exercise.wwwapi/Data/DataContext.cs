using exercise.wwwapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace exercise.wwwapi.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            // Constructor that accepts DbContextOptions to configure the context
        }
        public DbSet<Product> Products { get; set; }  // Represents a collection of Product entities in the database
    }
}