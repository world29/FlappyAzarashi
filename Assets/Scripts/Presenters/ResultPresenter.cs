using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ResultPresenter : MonoBehaviour, IGameStateEventHandler
{
    public Button m_continueButton;
    public Button m_showAdsButton;
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
        if (GameDataAccessor.PlayerLifeCount == 0)
        {
            m_showAdsButton.gameObject.SetActive(true);
        }
        else
        {
            m_continueButton.gameObject.SetActive(true);
        }

        m_surrenderButton.gameObject.SetActive(true);

        m_onEnterResultState.Invoke();
    }

    void OnExitResultState()
    {
        m_onExitResultState.Invoke();

        m_surrenderButton.gameObject.SetActive(false);
        m_showAdsButton.gameObject.SetActive(false);
        m_continueButton.gameObject.SetActive(false);
    }
}
