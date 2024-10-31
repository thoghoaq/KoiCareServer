namespace KoiCare.Domain.Entities
{
    public class KoiGroup
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<KoiType> KoiTypes { get; set; } = [];
        public ICollection<ServingSize> ServingSizes { get; set; } = [];
    }
}
