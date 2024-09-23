using System.ComponentModel.DataAnnotations;

namespace KoiCare.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        [MaxLength(32)]
        public required string Name { get; set; }

        public virtual ICollection<User> Users { get; set; } = [];
    }
}
