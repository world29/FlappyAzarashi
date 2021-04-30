using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ResultPresenter : MonoBehaviour, IGameStateEventHandler
{
    public ScoreCounter m_scoreCounter;

    public Button m_surrenderButton;

    public UnityEvent m_onEnterResultState;
    public UnityEvent m_onExitResultState;

    private void OnEnable()
    {
        BroadcastReceivers.RegisterBroadcastReceiver<IGameStateEventHandler>(gameObject);
    }

    private void OnDisable()
    {
        BroadcastReceivers.UnregisterBroadcastReceiver<IGameStateEventHandler>(gameObject);
    }

    public void OnGameStateChanged(IGameState prevState, IGameState nextState)
    {
        if (nextState is GameState_Result)
        {
            OnEnterResultState();
        }
        else if (prevState is GameState_Result)
        {
            OnExitResultState();
        }
    }

    void OnEnterResultState()
    {
        m_scoreCounter.gameObject.SetActive(true);

        m_surrenderButton.gameObject.SetActive(true);

        m_onEnterResultState.Invoke();
    }

    void OnExitResultState()
    {
        m_onExitResultState.Invoke();

        m_surrenderButton.gameObject.SetActive(false);

        m_scoreCounter.gameObject.SetActive(false);
    }
}
