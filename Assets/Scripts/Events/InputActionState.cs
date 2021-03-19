using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputActionState
{
    bool IsAcitonEnter(string actionName);
}

public class InputActionState : MonoBehaviour
{
    public bool isEnter = false;
}
