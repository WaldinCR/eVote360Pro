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
         services.AddScoped<IElectionService, ElectionService>();
         services.AddScoped<ICitizenService, CitizenService>();
         services.AddScoped<IElectivePositionService, ElectivePositionService>();
         services.AddScoped<ICandidatePositionService, CandidatePositionService>();
         #endregion

        }

    }
}
