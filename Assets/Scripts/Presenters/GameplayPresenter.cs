using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayPresenter : MonoBehaviour, IGameStateEventHandler
{
    public GameObject m_gameplayUI;

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
        // Ready -> Gameplay ステートの間は UI を表示する
        if (nextState is GameState_GameplayReady)
        {
            OnEnterReadyState();
        }

        if (prevState is GameState_GameplayMain)
        {
            OnExitGameplayState();
        }
    }

    void OnEnterReadyState()
    {
        m_gameplayUI.SetActive(true);
    }

    void OnExitGameplayState()
    {
        m_gameplayUI.SetActive(false);
    }
}
