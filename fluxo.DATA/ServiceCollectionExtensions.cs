using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace fluxo.DATA
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEntityFramework(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlite(connectionString));
        }
    }
}