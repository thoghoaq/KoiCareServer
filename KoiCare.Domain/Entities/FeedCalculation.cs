namespace KoiCare.Domain.Entities
{
    public class FeedCalculation
    {
        public int Id { get; set; }
        public int KoiIndividualId { get; set; }
        public decimal DailyAmount { get; set; }
        public decimal Frequency { get; set; }
        public int Type { get; set; }
        public DateTime CalculationDate { get; set; }

        public virtual KoiIndividual KoiIndividual { get; set; } = null!;
    }
}
