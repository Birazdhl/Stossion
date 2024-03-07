using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Stossion.Web.Authorization
{
		public class AuthorizeAPI : Attribute, IAuthorizationFilter
		{
			public void OnAuthorization(AuthorizationFilterContext context)
			{
				// Your custom authorization logic goes here
				if (!IsUserAuthorized(context.HttpContext.User))
				{
					// If the user is not authorized, redirect or return an unauthorized response
					context.Result = new ForbidResult();
				}
			}

			private bool IsUserAuthorized(System.Security.Principal.IPrincipal user)
			{
				// Your custom authorization logic goes here
				// For example, check roles, claims, etc.
				return user.IsInRole("Admin");
			}
	}
}
