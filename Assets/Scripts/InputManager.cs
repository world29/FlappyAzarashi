using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static PlayerAction Input { get; private set; }

    private void Awake()
    {
        Debug.Assert(Input == null);

        Input = new PlayerAction();
    }

    private void OnEnable()
    {
        Input.Enable();
    }

    private void OnDisable()
    {
        Input.Disable();
    }

    private void OnDestroy()
    {
        Input.Disable();

        Input = null;
    }
}
