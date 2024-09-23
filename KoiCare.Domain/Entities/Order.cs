namespace KoiCare.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }

        public virtual User Customer { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = [];
    }
}
