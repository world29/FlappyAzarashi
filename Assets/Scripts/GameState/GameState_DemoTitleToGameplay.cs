using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameState_DemoTitleToGameplay : IGameState
{
    public void OnEnter(GameStateMachineContext ctx)
    {
        TimelineManager.Instance.m_timelineTitleToGameplay.Play();
    }

    public void OnExit(GameStateMachineContext ctx)
    {
    }

    public IGameState OnUpdate(GameStateMachineContext ctx)
    {
        if (TimelineManager.Instance.m_timelineTitleToGameplay.state != PlayState.Playing)
        {
            return new GameState_DemoPlayerSpawn();
        }

        return this;
    }
}
