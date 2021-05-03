using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyPresenter : MonoBehaviour, IGameStateEventHandler
{
    public GameObject m_readyUI;

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
        m_readyUI.SetActive(true);
    }

    void OnExitReadyState()
    {
        m_readyUI.SetActive(false);
    }
}
