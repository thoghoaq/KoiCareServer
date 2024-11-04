namespace KoiCare.Domain.Entities
{
    public class BlogPost
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public string? Image { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User Author { get; set; } = null!;
    }
}
