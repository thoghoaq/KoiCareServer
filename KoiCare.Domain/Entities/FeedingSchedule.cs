namespace KoiCare.Domain.Entities
{
    public class FeedingSchedule
    {
        public int Id { get; set; }
        public int PondId { get; set; }
        public decimal Amount { get; set; }
        public decimal Period { get; set; }

        public virtual Pond Pond { get; set; } = null!;
    }
}
