using Cysharp.Threading.Tasks;
using MessagePipe;
using System;
using System.Threading;

namespace Bravasoft
{
    public sealed class MessagePipeMediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public MessagePipeMediator(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        interface IRequestHandlerWrapper<TResponse>
        {
            TResponse Handle();
        }

        interface IAsyncRequestHandlerWrapper<TResponse>
        {
            UniTask<TResponse> Handle();
        }

        readonly struct RequestHandlerWrapper<TRequest, TResponse> : IRequestHandlerWrapper<TResponse>
        {
            private readonly IRequestHandler<TRequest, TResponse> _requestHandler;
            private readonly TRequest _request;

            public RequestHandlerWrapper(object requestHandler, object request)
            {
                _requestHandler = (IRequestHandler<TRequest, TResponse>)requestHandler;
                _request = (TRequest)request;
            }
            public TResponse Handle() => _requestHandler.Invoke(_request);
        }

        readonly struct AsyncRequestHandlerWrapper<TRequest, TResponse> : IAsyncRequestHandlerWrapper<TResponse>
        {
            private readonly IAsyncRequestHandler<TRequest, TResponse> _requestHandler;
            private readonly TRequest _request;

            public AsyncRequestHandlerWrapper(object requestHandler, object request)
            {
                _requestHandler = (IAsyncRequestHandler<TRequest, TResponse>)requestHandler;
                _request = (TRequest)request;
            }
            public UniTask<TResponse> Handle() => _requestHandler.InvokeAsync(_request);
        }

        public TResponse Send<TResponse>(IRequest<TResponse> request)
        {
            IRequestHandlerWrapper<TResponse> wrapper = GetWrapper(request);
            return wrapper.Handle();
        }

        public UniTask<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default) =>
            GetAsyncWrapper(request).Handle();

        private IRequestHandlerWrapper<TResponse> GetWrapper<TResponse>(IRequest<TResponse> request)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var hander = _serviceProvider.GetRequiredService(handlerType);
            var wrapperType = typeof(RequestHandlerWrapper<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var wrapper = (IRequestHandlerWrapper<TResponse>)Activator.CreateInstance(wrapperType, hander, request);
            return wrapper;
        }

        private IAsyncRequestHandlerWrapper<TResponse> GetAsyncWrapper<TResponse>(IRequest<TResponse> request)
        {
            var handlerType = typeof(IAsyncRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var hander = _serviceProvider.GetRequiredService(handlerType);
            var wrapperType = typeof(AsyncRequestHandlerWrapper<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var wrapper = (IAsyncRequestHandlerWrapper<TResponse>)Activator.CreateInstance(wrapperType, hander, request);
            return wrapper;
        }          
    }
}
