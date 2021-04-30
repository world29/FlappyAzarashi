using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState_Continue : IGameState
{
    class GameEventListener : IGameEventListener
    {
        public bool Raised { get; private set; } = false;

        public void OnEventRaised()
        {
            Raised = true;
        }
    }

    GameEventListener m_continueListener;

    public void OnEnter(GameStateMachineContext ctx)
    {
        m_continueListener = new GameEventListener();

        ctx.m_PressContinueEvent.RegisterListener(m_continueListener);
    }

    public void OnExit(GameStateMachineContext ctx)
    {
        ctx.m_PressContinueEvent.UnregisterListener(m_continueListener);

        m_continueListener = null;
    }

    public IGameState OnUpdate(GameStateMachineContext ctx)
    {
        if (m_continueListener.Raised)
        {
            return new GameState_DemoPlayerRespawn();
        }

        return this;
    }
}
