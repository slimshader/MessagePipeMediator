using Bravasoft;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainLifetimeScope : LifetimeScope
{
    [SerializeField]
    private PingHandler _pingHandler;

    protected override void Configure(IContainerBuilder builder)
    {
        var options = builder.RegisterMessagePipe();

        builder.RegisterRequestHandler<Add, Sum, AddHandler>(options);
        builder.RegisterInstance<IRequestHandler<Ping, Pong>>(_pingHandler);


        builder.Register<IMediator, MessagePipeMediator>(Lifetime.Singleton);
    }
}
