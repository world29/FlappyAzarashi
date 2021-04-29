using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHandler : MonoBehaviour
{
    public UnityEvent[] m_events;

    public void Invoke(int index)
    {
        if (index >= m_events.Length) return;

        m_events[index].Invoke();
    }
}
