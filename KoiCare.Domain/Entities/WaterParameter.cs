namespace KoiCare.Domain.Entities
{
    public class WaterParameter
    {
        public int Id { get; set; }
        public int PondId { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Salinity { get; set; }
        public decimal? Ph { get; set; }
        public decimal? Oxygen { get; set; }
        public decimal? NO2 { get; set; }
        public decimal? NO3 { get; set; }
        public decimal? PO4 { get; set; }
        public DateTime MeasuredAt { get; set; }

        public virtual Pond Pond { get; set; } = null!;
    }
}
