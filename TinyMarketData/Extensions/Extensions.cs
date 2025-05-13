using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinyMarketCore.Interfaces;
using TinyMarketCore.Services;
using TinyMarketData.Repositories;

namespace TinyMarketData.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// se agregan todos los repositorios
        /// </summary>
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            // cadena de conexión a inyectar
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddScoped<ICategoryRepository>(sp => new CategoryRepository(connectionString));
            services.AddScoped<IProductRepository>(sp => new ProductRepository(connectionString));

            return services;
        }

        /// <summary>
        /// se agregan todos los services
        /// </summary>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
