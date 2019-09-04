using Automapper.Extensions;
using AutoMapper;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Automapper.Extensions.Projections {
    public class AutoMapperTypeAdapterFactory : ITypeAdapterFactory {


        private IMapper _mapper;

        public AutoMapperTypeAdapterFactory(bool assertConfigurationIsValid, bool compileMappings) {

            var profiles = GetAssemblies()
                .SelectMany(p => p.GetTypes())
                .Where(p => p.GetTypeInfo().BaseType == typeof(Profile));

            var configuration = new MapperConfiguration(cfg => {
                cfg.AllowNullCollections = true;
                cfg.AllowNullDestinationValues = true;
                foreach (var profile in profiles) {
                    cfg.AddProfile(Activator.CreateInstance(profile) as Profile);
                }
            });


            if (assertConfigurationIsValid)
                configuration.AssertConfigurationIsValid();
            if (compileMappings)
                configuration.CompileMappings();

            _mapper = configuration.CreateMapper();
        }

        public ITypeAdapter Create() {
            return new AutoMapperTypeAdapter(_mapper);
        }

        private Assembly[] GetAssemblies() {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries.Where(p => p.Type.Equals("Project", StringComparison.CurrentCultureIgnoreCase));
            foreach (var library in dependencies) {
                var name = new AssemblyName(library.Name);
                var assembly = Assembly.Load(name);
                assemblies.Add(assembly);
            }
            return assemblies.ToArray();
        }
    }
}
