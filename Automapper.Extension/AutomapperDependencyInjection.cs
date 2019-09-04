using Microsoft.Extensions.DependencyInjection;

namespace Automapper.Extensions.Projections {
    public static class AutomapperDependencyInjection {

        public static void AddAutomapper(this IServiceCollection services, bool assertConfigurationIsValid, bool compileMappings) {

            services.AddTransient<ITypeAdapterFactory>(c =>
                    new AutoMapperTypeAdapterFactory(assertConfigurationIsValid, compileMappings));

            var serviceProvider = services.BuildServiceProvider();
            TypeAdapterFactory.Set(serviceProvider.GetService<ITypeAdapterFactory>());

        }
    }
}
