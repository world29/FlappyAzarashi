using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IGameStateEventHandler : IEventSystemHandler
{
    void OnGameStateChanged(IGameState prevState, IGameState nextState);
}
