

using CustomMediatR.library.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Security.Cryptography;

namespace CustomMediatR.library.Extensions
{
    public static class ServiceRegistrationExtension
    {
        public static IServiceCollection AddMediatorServices(this IServiceCollection services, Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(i => i.GetTypes());

            var requestHandlers = RequestHandlerTypes(types);

            foreach (var handler in requestHandlers)
            {
                var handlerInterface = handler.GetInterfaces().FirstOrDefault();
                var requestType = handlerInterface.GetGenericArguments()[0];
                var responseType = handlerInterface.GetGenericArguments()[1];

                var genericType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

                services.AddTransient(genericType, handler);
            }

            services.AddSingleton<IMediator, Mediator>();

            return services;
        }
        public static IServiceProvider AddServiceProvider(this IServiceProvider serviceProvider)
        {
            MyServiceProvider.SetService(serviceProvider);
            return serviceProvider;
        }

        static IEnumerable<Type> RequestHandlerTypes(IEnumerable<Type> types)
        {
           
          var _types = types.Where(i => i.FullName.EndsWith("Handler")).ToList();

            foreach (var _type in _types)
            {
                var _interface = _type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                    .FirstOrDefault();

                if (_interface != null)
                    yield return _type;
            }
        }

    }
}
