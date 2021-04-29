using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameState
{
    void OnEnter(GameStateMachineContext ctx);

    void OnExit(GameStateMachineContext ctx);

    IGameState OnUpdate(GameStateMachineContext ctx);
}
