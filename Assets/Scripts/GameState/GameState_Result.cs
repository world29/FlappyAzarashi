using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState_Result : IGameState
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
    GameEventListener m_surrenderListener;

    public void OnEnter(GameStateMachineContext ctx)
    {
        m_continueListener = new GameEventListener();
        m_surrenderListener = new GameEventListener();

        ctx.m_PressContinueEvent.RegisterListener(m_continueListener);
        ctx.m_PressSurrenderEvent.RegisterListener(m_surrenderListener);
    }

    public void OnExit(GameStateMachineContext ctx)
    {
        ctx.m_PressSurrenderEvent.UnregisterListener(m_surrenderListener);
        ctx.m_PressContinueEvent.UnregisterListener(m_continueListener);

        m_surrenderListener = null;
        m_continueListener = null;
    }

    public IGameState OnUpdate(GameStateMachineContext ctx)
    {
        if (m_continueListener.Raised)
        {
            return new GameState_DemoPlayerRespawn();
        }
        else if (m_surrenderListener.Raised)
        {
            //todo: 本来はステートマシンの外部でシーン遷移を呼び出す
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            return new GameState_Title();
        }

        return this;
    }
}
