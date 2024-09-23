namespace KoiCare.Domain.Entities
{
    public class KoiGrowth
    {
        public int Id { get; set; }
        public int KoiIndividualId { get; set; }
        public decimal Length { get; set; }
        public decimal Weight { get; set; }
        public DateTime MeasuredAt { get; set; }

        public virtual KoiIndividual KoiIndividual { get; set; } = null!;
    }
}
