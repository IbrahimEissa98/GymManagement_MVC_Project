using GymManagement_MVC_Project.BLL.Services;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement_MVC_Project.BLL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGymBusinessLogic(this IServiceCollection services)
    {
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IPlanService, PlanService>();
        services.AddScoped<ITrainerService, TrainerService>();
        

        return services;
    }
}
