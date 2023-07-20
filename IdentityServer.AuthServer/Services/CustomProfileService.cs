using IdentityServer.AuthServer.Repositories;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityServer.AuthServer.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly ICustomUserRepository _repository;

        public CustomProfileService(ICustomUserRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// UserInfo endpoimtine istek atılınca çalışır
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subId = context.Subject.GetSubjectId();

            var user = await _repository.FindById(Convert.ToInt32(subId));

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName), // Controller da User.Identity.Name diyip alıcam mapleme önemli
                // new Claim("username", user.UserName) // o zaman User.Claims.First(x=>x.Type=="username").Value ile alabilirm.
                new Claim("city", user.City)
            };

            if (user.Id == 1)
            {
                claims.Add(new Claim("role", "admin")); // "role" önemli client programda RoleClaimType="role" dediği için "role" aynı olmalı.
            }
            else
            {
                claims.Add(new Claim("role", "customer"));
            }

            context.AddRequestedClaims(claims); // cookieye ekler

            //context.IssuedClaims = claims; // claimleri jwt içine gömer. ama token mümkün olduğunca hafif olsun sadece scopelar kalsın ve user bilgileri userInfo endpointten gelsin.
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var userId = context.Subject.GetSubjectId();

            var user = await _repository.FindById(Convert.ToInt32(userId));

            context.IsActive = user != null;
        }
    }
}
