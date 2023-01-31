using Bravasoft;
using UnityEngine;
using VContainer;

public class MediatorTest : MonoBehaviour
{
    [Inject]
    IMediator _mediator;

    void Start()
    {
        var pong = _mediator.Send(new Ping() { Msg = "yolo" });
        Debug.Log(pong.Msg);

        var sum = _mediator.Send(new Add() { A = 1, B = 2 });
        Debug.Log(sum.Value);
    }
}
