using MessagePipe;
using UnityEngine;

public class PingHandler : MonoBehaviour, IRequestHandler<Ping, Pong>
{
    Pong IRequestHandlerCore<Ping, Pong>.Invoke(Ping request)
    {
        return new Pong() { Msg = "Hello from handler!" };
    }
}
