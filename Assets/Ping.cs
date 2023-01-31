using Bravasoft;
using MessagePipe;

public sealed class Ping : IRequest<Pong>
{
    public string Msg { get; set; }
}

public sealed class Pong
{
    public string Msg { get; set; }
}

public sealed class Add : IRequest<Sum>
{
    public int A { get; set; }
    public int B { get; set; }
}

public sealed class Sum
{
    public int Value { get; set; }
}

public sealed class AddHandler : IRequestHandler<Add, Sum>
{
    Sum IRequestHandlerCore<Add, Sum>.Invoke(Add request) => new Sum() { Value = request.A + request.B };
}
