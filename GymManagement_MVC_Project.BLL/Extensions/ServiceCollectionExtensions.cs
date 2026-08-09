using GymManagement_MVC_Project.BLL.Providers;
using GymManagement_MVC_Project.BLL.Providers.Contracts;
using GymManagement_MVC_Project.BLL.Services;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace GymManagement_MVC_Project.BLL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGymBusinessLogic(this IServiceCollection services, string autoMapperLicenseKey)
    {
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IPlanService, PlanService>();
        services.AddScoped<ITrainerService, TrainerService>();

        //services.AddAutoMapper(config =>
        //{
        //    config.LicenseKey = autoMapperLicenseKey;
        //},
        //typeof(PlanProfile).Assembly,);

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
