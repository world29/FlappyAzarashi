using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyPresenter : MonoBehaviour, IGameStateEventHandler
{
    public Text m_readyText;

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
        if (nextState is GameState_GameplayReady)
        {
            OnEnterReadyState();
        }
        else if (prevState is GameState_GameplayReady)
        {
            OnExitReadyState();
        }
    }

    void OnEnterReadyState()
    {
        m_readyText.gameObject.SetActive(true);
    }

    void OnExitReadyState()
    {
        m_readyText.gameObject.SetActive(false);
    }
}
