using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachineBehaviour : MonoBehaviour
{
    public GameStateMachineContext m_gameContext;

    private IGameState m_currentState = null;

    void InitializeState()
    {
        ChangeState(new GameState_Title());
    }

    void ChangeState(IGameState nextState)
    {
        var prevState = m_currentState;

        if (prevState != null)
        {
            Debug.Log("State exit: " + prevState.ToString());

            prevState.OnExit(m_gameContext);
        }

        if (nextState != null)
        {
            Debug.Log("State enter: " + nextState.ToString());

            nextState.OnEnter(m_gameContext);
        }

        m_currentState = nextState;

        BroadcastExecuteEvents.Execute<IGameStateEventHandler>(null /* eventData */,
            (handler, eventData) => handler.OnGameStateChanged(prevState, nextState));
    }

    private void OnEnable()
    {
        InitializeState();
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        if (m_currentState == null) return;

        var nextState = m_currentState.OnUpdate(m_gameContext);
        if (nextState != m_currentState)
        {
            ChangeState(nextState);
        }
    }
}
