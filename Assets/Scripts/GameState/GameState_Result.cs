using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

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

    GameEventListener m_surrenderListener;

    private bool m_isAdvertisementCompleted = false;

    public void OnEnter(GameStateMachineContext ctx)
    {
        m_surrenderListener = new GameEventListener();

        ctx.m_PressSurrenderEvent.RegisterListener(m_surrenderListener);
    }

    public void OnExit(GameStateMachineContext ctx)
    {
        ctx.m_PressSurrenderEvent.UnregisterListener(m_surrenderListener);

        m_surrenderListener = null;
    }

    public IGameState OnUpdate(GameStateMachineContext ctx)
    {
        if (m_isAdvertisementCompleted)
        {
            return new GameState_Title();
        }

        if (m_surrenderListener.Raised)
        {
            if (!Advertisement.IsReady())
            {
                ReloadScene();

                return new GameState_Title();
            }

            var options = new ShowOptions
            {
                resultCallback = OnAdvertisementCompleted
            };

            Advertisement.Show(options);
        }

        return this;
    }

    public void OnAdvertisementCompleted(ShowResult result)
    {
        ReloadScene();

        m_isAdvertisementCompleted = true;
    }

    private void ReloadScene()
    {
        //todo: 本来はステートマシンの外部でシーン遷移を呼び出す
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
