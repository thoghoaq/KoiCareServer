namespace KoiCare.Domain.Entities
{
    public class FeedingSchedule
    {
        public int Id { get; set; }
        public int KoiIndividualId { get; set; }
        public decimal Amount { get; set; }
        public decimal Period { get; set; }

        public virtual KoiIndividual KoiIndividual { get; set; } = null!;
    }
}
