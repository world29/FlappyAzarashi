using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ResultPresenter : MonoBehaviour, IGameStateEventHandler
{
    public GameObject m_resultUI;

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
        m_resultUI.SetActive(true);

        m_resultUI.GetComponent<ResultSequence>().Animate();

        m_onEnterResultState.Invoke();
    }

    void OnExitResultState()
    {
        m_onExitResultState.Invoke();

        m_resultUI.SetActive(false);
    }
}
