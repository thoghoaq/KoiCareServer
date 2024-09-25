using KoiCare.Application.Abtractions.Database;
using KoiCare.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace KoiCare.Infrastructure.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthAttribute(string? role = null) : Attribute, IAuthorizationFilter
    {
        public string? Role { get; set; } = role;
        public bool AllowAnonymous { get; set; } = false;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (AllowAnonymous)
            {
                return;
            }

            var token = context.HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            if (token.IsNullOrEmpty())
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            if (jwtToken?.ValidTo < DateTime.UtcNow)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userId = jwtToken?.Payload?.Sub;
            var repos = context.HttpContext.RequestServices.GetService<IRepository<User>>();
            var user = repos?.Queryable().FirstOrDefault(u => u.IdentityId == userId);
            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!user.IsActive)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (Role.IsNullOrEmpty())
            {
                return;
            }

            var roles = Role!.Split(",");

            if (!roles!.Contains(user.Role.Name))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            return;
        }
    }
}
