using Microsoft.AspNetCore.Identity;
using RealEstate.Application.BusinessLogic;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Interfaces;
using RealEstate.Shared.Options;
using RealEstate.Shared.Utils;

namespace RealEstate.API.App_Start
{
    internal static class DIConfiguration
    {
        internal static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient(typeof(IReplaceVisitor), typeof(ReplaceVisitor));
            services.AddTransient(typeof(ITreeModifier), typeof(TreeModifier));
            services.AddTransient(typeof(IPropertyBusinessLogic), typeof(PropertyBusinessLogic));
        }

        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PasswordOptions>(configuration.GetSection("PasswordOptions"));
            services.Configure<PaginationOptions>(configuration.GetSection("PaginationOptions"));

            return services;
        }
    }
}
