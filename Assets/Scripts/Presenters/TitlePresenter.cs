using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePresenter : MonoBehaviour, IGameStateEventHandler
{
    public GameObject m_titleUI;

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
        if (nextState is GameState_Title)
        {
            OnEnterTitleState();
        }
        else if (prevState is GameState_Title)
        {
            OnExitTitleState();
        }
    }

    void OnEnterTitleState()
    {
        m_titleUI.gameObject.SetActive(true);
    }

    void OnExitTitleState()
    {
        m_titleUI.gameObject.SetActive(false);
    }
}
