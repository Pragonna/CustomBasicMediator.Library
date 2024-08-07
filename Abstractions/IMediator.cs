﻿namespace CustomMediatR.library.Abstractions
{
    public interface IMediator
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request);
    }
}
