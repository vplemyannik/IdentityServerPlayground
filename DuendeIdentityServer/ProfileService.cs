using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using DuendeIdentityServer.Quickstart;
using Microsoft.AspNetCore.Identity;

namespace DuendeIdentityServer
{
    public class ProfileService : IProfileService
    {
        
        public ProfileService()
        {
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //>Processing
            var user = TestUsers.Users.First(x => x.SubjectId == context.Subject.GetSubjectId());

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
            };

            context.IssuedClaims.AddRange(claims);
            
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}