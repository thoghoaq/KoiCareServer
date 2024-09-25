namespace KoiCare.Application.Abtractions.LoggedUser
{
    public interface ILoggedUser
    {
        public int UserId { get; }
        public string UserName { get; }
        public string Email { get; }
        public int RoleId { get; }
        public string RoleName { get; }
        public string IdentityId { get; }
        public string IpAddress { get; }
        public bool IsAuthenticated { get; }
        public bool IsAdmin { get; }
    }
}
