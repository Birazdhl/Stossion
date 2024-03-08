using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Stossion.Web.Authorization
{
	public class CustomAuthorizeAttribute : TypeFilterAttribute
	{
		public CustomAuthorizeAttribute() : base(typeof(CustomAuthorizeFilter))
		{
		}
	}

	public class CustomAuthorizeFilter(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			// Your custom authorization logic goes here
			if (!IsAuthorized(context))
			{
				// If not authorized, set the result to access denied
				context.Result = new ForbidResult();
			}
		}

		private bool IsAuthorized(AuthorizationFilterContext context)
		{
			string? authorizationToken = httpContextAccessor?.HttpContext?.Request.Cookies["AuthorizationToken"];

			TokenValidationParameters validationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
				ValidAudience = configuration.GetValue<string>("Jwt:Audience"),
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key")!))
			};

			// Try to validate and decode the token
			try
			{
				JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
				ClaimsPrincipal principal = tokenHandler.ValidateToken(authorizationToken, validationParameters, out SecurityToken validatedToken);

				// Access claims from the principal
				var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				var userName = principal.FindFirst(ClaimTypes.Name)?.Value;

				// Your logic with decoded claims

			}
			catch (SecurityTokenException ex)
			{
			}

			return context.HttpContext.User.Identity.IsAuthenticated;
		}
	}
}
