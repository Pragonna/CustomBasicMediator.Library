using CustomMediatR.library.Abstractions;

namespace CustomMediatR.library
{
    public class Mediator : IMediator
    {
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            var requestType = request.GetType();
            var requestInterface = requestType
                                            .GetInterfaces()
                                            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>))
                                            .FirstOrDefault();

            var responseType = requestInterface.GenericTypeArguments[0];

            var genericType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

            var handler = MyServiceProvider.ServiceProvider.GetService(genericType);

            return (Task<TResponse>)genericType.GetMethod("Handle").Invoke(handler, new object[] { request });
        }
    }
}
