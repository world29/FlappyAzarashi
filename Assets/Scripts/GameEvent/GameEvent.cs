using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    private List<IGameEventListener> m_listeners = new List<IGameEventListener>();

    public void Raise()
    {
        for (int i = m_listeners.Count - 1; i >= 0; i--)
        {
            m_listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(IGameEventListener listener)
    {
        m_listeners.Add(listener);
    }

    public void UnregisterListener(IGameEventListener listener)
    {
        m_listeners.Remove(listener);
    }
}
