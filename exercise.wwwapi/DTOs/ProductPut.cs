namespace exercise.wwwapi.DTOs
{
    public class ProductPut
    {
        // I have chosen to make it possible to update only the fields that the client wants to update, so all fields are optional. The id should never be changed by the client
        public string? Name { get; set; }
        public string? Category { get; set; }
        public int? Price { get; set; }
    }
}