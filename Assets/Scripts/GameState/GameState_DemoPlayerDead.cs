using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameState_DemoPlayerDead : IGameState
{
    private PlayableDirector m_playingTimeline;

    public void OnEnter(GameStateMachineContext ctx)
    {
        if (GameDataAccessor.PlayerLifeCount == 0)
        {
            m_playingTimeline = TimelineManager.Instance.m_timelinePlayerDeadAndGameOver;
        }
        else
        {
            m_playingTimeline = TimelineManager.Instance.m_timelinePlayerDead;
        }

        var sound = Sound.GetInstance();

        sound.StopBgm();

        m_playingTimeline.Play();
    }

    public void OnExit(GameStateMachineContext ctx)
    {

    }

    public IGameState OnUpdate(GameStateMachineContext ctx)
    {
        if (m_playingTimeline.state != PlayState.Playing)
        {
            return new GameState_Result();
        }

        return this;
    }
}
