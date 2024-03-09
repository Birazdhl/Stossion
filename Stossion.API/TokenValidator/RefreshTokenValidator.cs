using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Stossion.API.TokenValidator
{
		public class RefreshTokenValidator(IConfiguration configuration)
		{
			public bool Validate(string refreshToken)
			{
				TokenValidationParameters validationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,
					ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
					ValidAudience = configuration.GetValue<string>("Jwt:Audience"),
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:RefreshTokenSecret")!)),
					ClockSkew = TimeSpan.Zero
				};
				JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
				try
				{
					tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
					return true;
				}
				catch (Exception)
				{
					return false;
				}
			}
	}
}
