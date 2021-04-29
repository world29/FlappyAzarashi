using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public enum ActorId
{
    Otafuku,
    Tengu,
    Hyottoco,
}

public interface IActorEvents : IEventSystemHandler
{
    void OnActorDied(ActorId id);
}
