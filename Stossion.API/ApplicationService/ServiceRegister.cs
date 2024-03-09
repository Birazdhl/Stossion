using Stossion.BusinessLayers.Interfaces;
using Stossion.BusinessLayers.Services;

namespace Stossion.API.ApplicationService
{
    public static class ServiceRegistrer
    {
        public static void AddServices(this IServiceCollection service)
        {
            // Add your repository and other services here
            service.AddScoped<IUserInterface, UserService>();
            service.AddScoped<ICountryInterface, CountryService>();
			service.AddScoped<ITokenInterface, TokenService>();
            service.AddScoped<IDapperDbContext, DapperDbContext>();
			service.AddScoped<IDapperInterface, DapperRepository>();


		}
	}
}
