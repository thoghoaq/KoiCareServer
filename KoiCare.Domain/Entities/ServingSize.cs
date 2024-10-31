using KoiCare.Domain.Enums;

namespace KoiCare.Domain.Entities
{
    public class ServingSize
    {
        public int Id { get; set; }
        public EAgeRange AgeRange { get; set; }
        public decimal WeightPercent { get; set; }
        public required string FoodDescription { get; set; }
        public required string DailyFrequency { get; set; }
        public required int KoiGroupId { get; set; }

        public virtual KoiGroup KoiGroup { get; set; } = null!;
    }
}
