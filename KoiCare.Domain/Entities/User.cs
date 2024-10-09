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
        public int? GenderId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber {  get; set; }



        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Pond> Ponds { get; set; } = [];
        public virtual ICollection<BlogPost> BlogPosts { get; set; } = [];
        public virtual ICollection<Order> Orders { get; set; } = [];
        public virtual Gender? Gender { get; set; } 
    }
}
