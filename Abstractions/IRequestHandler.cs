namespace CustomMediatR.library.Abstractions
{
    public interface IRequestHandler<IRequest,TResponse> where IRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(IRequest request);
    }
}
