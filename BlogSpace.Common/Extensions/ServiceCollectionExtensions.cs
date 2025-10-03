using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogSpace.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            // You can register common helpers or configurations here
            return services;
        }
    }
}
