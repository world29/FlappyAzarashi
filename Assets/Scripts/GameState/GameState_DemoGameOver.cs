using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameState_DemoGameOver : IGameState
{
    private PlayableDirector m_playableDirector;

    public void OnEnter(GameStateMachineContext ctx)
    {
        m_playableDirector = TimelineManager.Instance.m_timelinePlayerGameOver;

        m_playableDirector.Play();
    }

    public void OnExit(GameStateMachineContext ctx)
    {
    }

    public IGameState OnUpdate(GameStateMachineContext ctx)
    {
        if (m_playableDirector.state != PlayState.Playing)
        {
            return new GameState_Result();
        }

        return this;
    }
}
