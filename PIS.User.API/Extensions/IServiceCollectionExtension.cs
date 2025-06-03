using PIS.User.Core.Services;

namespace PIS.User.API.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services)
        {
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IMenuMaster, MenuMasterService>();
            //services.AddScoped<IErrorLog, ErrorLogService>();
            return services;
        }
    }
}
