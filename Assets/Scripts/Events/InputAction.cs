using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IInputActionEventHandler : IEventSystemHandler
{
    void OnActionEnter(InputActionScriptableObject inputAction);
    //void OnActionStay();
    //void OnActionExit();
}
