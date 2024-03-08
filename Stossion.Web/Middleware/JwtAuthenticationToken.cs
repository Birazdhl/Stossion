using Microsoft.AspNetCore.Http;

namespace Stossion.Web.Middleware
{
	public class JwtTokenMiddleware(RequestDelegate _next, IHttpContextAccessor _httpContextAccessor)
	{
		
		public async Task Invoke(HttpContext context)
		{
			// Get the token from your token provider
			string? token = _httpContextAccessor?.HttpContext?.Request.Cookies["AuthorizationToken"];
			if (!string.IsNullOrEmpty(token))
			{
				// Add the token to the Authorization header
				context.Request.Headers.Add("Authorization", $"Bearer {token}");
			}

			// Call the next middleware in the pipeline
			await _next(context);
		}
	}
}
