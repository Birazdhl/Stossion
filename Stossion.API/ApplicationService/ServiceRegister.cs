using Stossion.BusinessLayers.Interfaces;
using Stossion.BusinessLayers.Services;

namespace Stossion.API.ApplicationService
{
    public static class ServiceRegistrer
    {
        public static void AddServices(this IServiceCollection service)
        {
            // Add your repository and other services here
            service.AddTransient<IUserInterface, UserService>();
            service.AddTransient<ICountryInterface, CountryService>();
			service.AddTransient<ITokenInterface, TokenService>();
            service.AddScoped<IDapperDbContext, DapperDbContext>();
			service.AddTransient<IDapperInterface, DapperRepository>();
			service.AddTransient<IEmailSenderService, EmailSenderService>();


		}
	}
}
