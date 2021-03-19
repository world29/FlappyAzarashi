using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputActionEventEmitter : MonoBehaviour
{
    public InputActionScriptableObject m_inputAction;

    public void OnActionEnter()
    {
        BroadcastExecuteEvents.Execute<IInputActionEventHandler>(null /* eventData */,
            (handler, eventData) => handler.OnActionEnter(m_inputAction));
    }
}
