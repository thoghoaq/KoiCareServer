namespace KoiCare.Domain.Entities
{
    public class SaltRequirement
    {
        public int Id { get; set; }
        public int PondId { get; set; }
        public decimal RequiredAmount { get; set; }

        public virtual Pond Pond { get; set; } = null!;
    }
}
