using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models.Interceptors;
using GymManagement_MVC_Project.DAL.Repositories.Plans;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GymManagement_MVC_Project.DAL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGymDataAccess(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IPlanRepository, PlanRepository>();
            services.AddSingleton<TimestampInterceptor>();
            services.AddDbContext<GymDbContext>((sp, options) =>
            {
                options.UseSqlServer(connectionString);
                options.LogTo(Console.WriteLine, LogLevel.Information);

                options.AddInterceptors(sp.GetRequiredService<TimestampInterceptor>());
            });

            return services;
        }
    }
}
