using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using fluxo.DATA.Context;
using fluxo.DATA.Repository;

namespace fluxo.DATA
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFluxoDataUsingSqlite(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlite(connectionString));

            services.AddRepositories();
        }
        public static void AddFluxoDataUsingMysql(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DataContext>(options => options.UseMySql(connectionString));

            services.AddRepositories();
        }

        private static void AddRepositories(this IServiceCollection services) {
            services.AddScoped<IAuthRepository, AuthRepository>();
        }
    }
}