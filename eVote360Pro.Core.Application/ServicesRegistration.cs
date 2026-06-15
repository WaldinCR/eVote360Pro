using eVote360Pro.Core.Application.Interfaces;
using eVote360Pro.Core.Application.Mappings.DtosAndViewModels;
using eVote360Pro.Core.Application.Mappings.EntitiesAndDtos;
using eVote360Pro.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace eVote360Pro.Core.Application
{
    public static class ServicesRegistration
    {
        //extension method - decorator pattern
        public static void AddApplicationLayerIoc(this IServiceCollection services)
        {
            #region Services IOC
            services.AddTransient<IUserService, UserService>();
            //services.AddTransient<IElectorService, ElectorService>();
            //waldin
            services.AddTransient<IPoliticalPartyService, PoliticalPartyService>();
            services.AddTransient<ICandidateService, CandidateService>();
            services.AddTransient<IPoliticalLeaderAssignmentService, PoliticalLeaderAssignmentService>();
            services.AddTransient<IAllianceService, AllianceService>();
            services.AddTransient<IAllianceRequestService, AllianceRequestService>();
            #endregion

            #region AutoMapper
            services.AddAutoMapper(cfg => {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });
            #endregion

        }

    }
}
