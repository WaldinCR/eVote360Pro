using eVote360Pro.Core.Domain.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using eVote360Pro.Infrastructure.Shared.Services;
using eVote360Pro.Core.Application.Interfaces;

namespace eVote360Pro.Infrastructure.Shared
{
   public static class ServicesRegistration
    {
        //extension method - decorator pattern
        public static void AddSharedLayerIoc(this IServiceCollection services, IConfiguration config)
        {
            #region Configurations
            services.Configure<MailSettings>(config.GetSection("MailSettings"));
            #endregion

            #region Services IOC
            services.AddScoped<IEmailService, EmailService>();
            #endregion

        }

    }
}
