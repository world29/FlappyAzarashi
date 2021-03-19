using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputActionStateManager : MonoBehaviour, IInputActionEventHandler, IInputActionState
{
    Dictionary<string, InputActionState> m_actionStateDict = new Dictionary<string, InputActionState>();

    // IInputActionEventHandler
    public void OnActionEnter(InputActionScriptableObject inputAction)
    {
        m_actionStateDict[inputAction.ActionName].isEnter = true;
    }

    // IInputActionState
    public bool IsAcitonEnter(string actionName)
    {
        if (m_actionStateDict.ContainsKey(actionName))
        {
            return m_actionStateDict[actionName].isEnter;
        }

        return false;
    }

    private void OnEnable()
    {
        BroadcastReceivers.RegisterBroadcastReceiver<IInputActionEventHandler>(gameObject);
        ServiceLocator.Register<IInputActionState>(this);
    }

    private void OnDisable()
    {
        //ServiceLocator.Register<IInputActionState>(null);
        BroadcastReceivers.UnregisterBroadcastReceiver<IInputActionEventHandler>(gameObject);
    }

    private void LateUpdate()
    {
        ResetState();
    }

    void ResetState()
    {
        foreach (var key in m_actionStateDict.Keys)
        {
            m_actionStateDict[key].isEnter = false;
        }
    }
}
