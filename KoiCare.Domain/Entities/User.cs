using System.ComponentModel.DataAnnotations;

namespace KoiCare.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        [MaxLength(255)]
        public required string Username { get; set; }
        [MaxLength(255), EmailAddress]
        public required string Email { get; set; }
        [MaxLength(64)]
        public required string IdentityId { get; set; }
        public bool IsActive { get; set; }

        public virtual Role Role { get; set; } = null!;
    }
}
