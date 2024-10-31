namespace KoiCare.Domain.Entities
{
    public class KoiIndividual
    {
        public int Id { get; set; }
        public int KoiTypeId { get; set; }
        public int PondId { get; set; }
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }
        public decimal? Age { get; set; }
        public decimal? Length { get; set; }
        public decimal? Weight { get; set; }
        public int? Gender { get; set; }
        public string? Origin { get; set; }
        public int? Shape { get; set; }
        public string? Breed { get; set; }

        public virtual KoiType KoiType { get; set; } = null!;
        public virtual Pond Pond { get; set; } = null!;
        public virtual ICollection<KoiGrowth> KoiGrowths { get; set; } = [];
        public virtual ICollection<FeedCalculation> FeedCalculations { get; set; } = [];
    }
}
