using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
using Hemapp.Extensions;
using Hemapp.Models;
using Hemapp.Models.Discgoldscore;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Hemapp.Providers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var user = userManager.Find(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("Fel", "Felaktigt email eller lösenord");
                context.Rejected();
            }
            ClaimsIdentity identity = await userManager.CreateIdentityAsync(user, context.Options.AuthenticationType);
            //var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            identity.AddClaim(new Claim("Username", context.UserName));
            var currentUser = userManager.FindById(identity.GetUserId());

            var userModel = new User
            {
                Email = identity.GetUserName(),
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,    
                UserId = identity.GetUserId()
            };
            
            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {
                    "user", userModel.ToJson()
                }
            });


            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(identity);
        }
    }
}