using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models.Interceptors;
using GymManagement_MVC_Project.DAL.Queries;
using GymManagement_MVC_Project.DAL.Queries.Contracts;
using GymManagement_MVC_Project.DAL.Repositories;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GymManagement_MVC_Project.DAL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGymDataAccess(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddSingleton<TimestampInterceptor>();

        services.AddScoped<ISessionQueryService, SessionQueryService>();
        services.AddScoped<IMembershipQueryService, MembershipQueryService>();
        services.AddScoped<IBookingQueryService, BookingQueryService>();

        services.AddDbContext<GymDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString);
            options.LogTo(Console.WriteLine, LogLevel.Information);

            options.AddInterceptors(sp.GetRequiredService<TimestampInterceptor>());
        });

        return services;
    }
}
