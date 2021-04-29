using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameState_DemoPlayerSpawn : IGameState
{
    public void OnEnter(GameStateMachineContext ctx)
    {
        TimelineManager.Instance.m_timelinePlayerSpawn.Play();
    }

    public void OnExit(GameStateMachineContext ctx)
    {
    }

    public IGameState OnUpdate(GameStateMachineContext ctx)
    {
        if (TimelineManager.Instance.m_timelinePlayerSpawn.state != PlayState.Playing)
        {
            return new GameState_GameplayReady();
        }

        return this;
    }
}
