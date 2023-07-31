using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ApiOrderFood.Models;
using ApiOrderFood.Services.User;
using ApiOrderFood.Services.Order;

namespace ApiOrderFood
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSqlServer<DatabaseContext>(Configuration.GetConnectionString("DefaultConnection"));
            services.AddTransient<IOrderServices, OrderServices>();
            services.AddTransient<IUsersServices, UsersServices>();
            return services;
        }

    }
}