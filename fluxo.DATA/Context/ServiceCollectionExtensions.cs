using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace fluxo.DATA.Context
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFluxoDataContextUsingSqlite(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlite(connectionString));
        }
    }
}