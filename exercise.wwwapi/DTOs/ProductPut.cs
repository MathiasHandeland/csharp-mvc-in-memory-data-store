namespace exercise.wwwapi.DTOs
{
    public class ProductPut
    {
        // product name, category and price fields is required by the client to fill out for adding a new product to the database
        public string Name { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }

        public int? Id { get; set; } // not required but optional
    }
}
