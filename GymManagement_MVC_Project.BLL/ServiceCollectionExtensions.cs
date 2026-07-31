using GymManagement_MVC_Project.BLL.Services;
using GymManagement_MVC_Project.BLL.Services.Contracts;
using GymManagement_MVC_Project.DAL.Data.Contexts;
using GymManagement_MVC_Project.DAL.Models.Interceptors;
using GymManagement_MVC_Project.DAL.Repositories;
using GymManagement_MVC_Project.DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GymManagement_MVC_Project.DAL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGymBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<IMemberService, MemberService>();
            

            return services;
        }
    }
}
