using Stossion.Domain;
using Stossion.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.BusinessLayers.Interfaces
{
	public interface ITokenInterface
	{
		string GenerateToken(UserSession user);
		string GenerateRefreshToken();
		bool ValidateToken(string refreshToken);
		Task Create(RefreshToken refreshToken);
		Task<RefreshToken> GetByToken(string token);
		Task<LoginResponse> GenerateAndReturnToken(string userName, bool isEmail = false);
		Task Delete(Guid id);
	}
}
