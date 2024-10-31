namespace KoiCare.Domain.Entities
{
    public class KoiType
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int? KoiGroupId { get; set; }

        public virtual ICollection<KoiIndividual> KoiIndividuals { get; set; } = [];
        public virtual ICollection<PerfectWaterVolume> PerfectWaterVolumes { get; set; } = [];
        public virtual KoiGroup? KoiGroup { get; set; }
    }
}
