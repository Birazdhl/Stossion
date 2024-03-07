using Stossion.Helpers.RestHelpers;
using Stossion.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.BusinessLayers.Interfaces
{
    public interface IUserInterface
    {
        Task<GeneralResponse> CreateUser(RegisterViewModel model);
        Task<LoginResponse> LoginUser(LoginViewModel model);
        string GetUserDetails();

    }
}
