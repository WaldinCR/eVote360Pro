using eVote360Pro.Infrastructure.Persistence.Contexts;
using eVote360Pro.Infrastructure.Persistence.Repositories;
using eVote360Pro.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eVote360Pro.Infrastructure.Persistence
{
    public static class ServicesRegistration
    {
        //extension method - decorator pattern
        public static void AddPersistenceLayerIoc(this IServiceCollection services, IConfiguration config)
        {
            #region contexts
            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<eVote360ProDbContext>(opt => opt.UseInMemoryDatabase("Evote360ProDb"));
                return;
            }
            else
             {
                 var connectionString = config.GetConnectionString("DefaultConnection");
                 services.AddDbContext<eVote360ProDbContext>(opt =>
                 opt.UseSqlServer(connectionString,
                  m=> m.MigrationsAssembly(typeof(eVote360ProDbContext).Assembly.FullName))
             , ServiceLifetime.Transient);
           
             }
            #endregion

            #region Repositories IOC
            #endregion
        }


    }
}

