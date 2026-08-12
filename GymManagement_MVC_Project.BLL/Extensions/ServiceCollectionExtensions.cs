using GymManagement_MVC_Project.BLL.Providers;
using GymManagement_MVC_Project.BLL.Providers.Contracts;
using GymManagement_MVC_Project.BLL.Services;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.DAL.Queries;
using GymManagement_MVC_Project.DAL.Queries.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement_MVC_Project.BLL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGymBusinessLogic(this IServiceCollection services)
    {
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IPlanService, PlanService>();
        services.AddScoped<ITrainerService, TrainerService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<ISessionQueryService, SessionQueryService>();

        services.AddScoped<ISessionQueryService, SessionQueryService>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
