namespace KoiCare.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }

        public virtual ICollection<Product> Products { get; set; } = [];
    }
}
