using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace eVote360Pro.Core.Application
{
    public static class ServicesRegistration
    {
        //extension method - decorator pattern
        public static void AddApplicationLayerIoc(this IServiceCollection services)
        {
            #region Services IOC
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IElectorService, ElectorService>();
            #endregion

            #region AutoMapper
            services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());
            #endregion

        }

    }
}
