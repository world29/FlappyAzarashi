using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerBehaviour : MonoBehaviour, IGameEventListener
{
    public GameEvent m_event;
    public UnityEvent m_response;

    private void OnEnable()
    {
        m_event.RegisterListener(this);
    }

    private void OnDisable()
    {
        m_event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        m_response.Invoke();
    }
}
