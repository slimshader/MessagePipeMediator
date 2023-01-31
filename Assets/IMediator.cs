using Cysharp.Threading.Tasks;
using System.Threading;

namespace Bravasoft
{
    public interface IRequest<TRespose> { }

    public interface IMediator
    {
        TResponse Send<TResponse>(IRequest<TResponse> request);
        UniTask<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }
}
