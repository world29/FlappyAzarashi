using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_GameplayReady : IGameState
{
    public void OnEnter(GameStateMachineContext ctx)
    {
        var sound = Sound.GetInstance();

        sound.PlayBgm("Stage");
    }

    public void OnExit(GameStateMachineContext ctx)
    {

    }

    public IGameState OnUpdate(GameStateMachineContext ctx)
    {
        if (InputManager.Input.PlatformAction.Jump.triggered)
        {
            return new GameState_GameplayMain();
        }

        return this;
    }
}
