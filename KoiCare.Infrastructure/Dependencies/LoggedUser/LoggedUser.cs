using KoiCare.Application.Abtractions.Database;
using KoiCare.Application.Abtractions.LoggedUser;
using KoiCare.Domain.Entities;
using KoiCare.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace KoiCare.Infrastructure.Dependencies.LoggedUser
{
    public class LoggedUser(IRepository<User> userRepos, IHttpContextAccessor httpContext) : ILoggedUser
    {
        private User? _user;

        private User? GetUser()
        {
            if (_user is not null)
            {
                return _user;
            }

            var token = httpContext.HttpContext?.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException();
            }
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            var userId = jwtToken?.Payload?.Sub;
            _user = userRepos.Queryable().Include(u => u.Role).FirstOrDefault(x => x.IdentityId == userId);
            return _user;
        }

        public int UserId => GetUser()!.Id;

        public string UserName => GetUser()!.Username;

        public string Email => GetUser()!.Email;

        public int RoleId => GetUser()!.RoleId;

        public string RoleName => GetUser()!.Role.Name;

        public string IdentityId => GetUser()!.IdentityId;

        public string IpAddress => httpContext.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

        public bool IsAuthenticated => GetUser() != null;

        public bool IsAdmin => GetUser()!.RoleId == (int)ERole.Admin;
    }
}
