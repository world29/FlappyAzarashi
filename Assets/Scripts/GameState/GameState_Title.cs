using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_Title : IGameState, IGameEventListener
{
    private bool m_stateExit = false;

    public void OnEnter(GameStateMachineContext ctx)
    {
        ctx.m_titlePressStartEvent.RegisterListener(this);
    }

    public void OnExit(GameStateMachineContext ctx)
    {
        ctx.m_titlePressStartEvent.UnregisterListener(this);

        m_stateExit = false;
    }

    public IGameState OnUpdate(GameStateMachineContext ctx)
    {
        if (m_stateExit)
        {
            return new GameState_DemoTitleToGameplay();
        }

        return this;
    }

    public void OnEventRaised()
    {
        m_stateExit = true;
    }
}
