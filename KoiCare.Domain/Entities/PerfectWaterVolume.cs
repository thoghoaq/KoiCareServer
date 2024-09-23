namespace KoiCare.Domain.Entities
{
    public class PerfectWaterVolume
    {
        public int Id { get; set; }
        public int KoiTypeId { get; set; }
        public decimal MinVolume { get; set; }
        public decimal MaxVolume { get; set; }
        public decimal OptimalVolume { get; set; }
        public decimal WaterLevel { get; set; }
        public decimal RecommendedSaltLevel { get; set; }

        public virtual KoiType KoiType { get; set; } = null!;
    }
}
